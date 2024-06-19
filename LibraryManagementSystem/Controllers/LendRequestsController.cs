using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.Controllers
{
    public class LendRequestsController : Controller
    {
        private readonly Library_Management_SystemContext _context;
        private readonly IAccountRepo _accountsRepo;
        private readonly ILendRepo _lendRepo;

        public LendRequestsController(Library_Management_SystemContext context, IAccountRepo accountRepo, ILendRepo lendRepo)
        {
            _context = context;
            _accountsRepo = accountRepo;
            _lendRepo = lendRepo;
        }

        // GET: LendRequests
        // Admin
        public async Task<IActionResult> Index()
        {
            var library_Management_SystemContext = _context.LendRequests.Where(l => l.LendStatus == "Requested").Include(b => b.Book).Include(u => u.User);
            return View(await library_Management_SystemContext.ToListAsync());
        }
        public async Task<IActionResult> LentBooks()
        {
            var library_Management_SystemContext = _context.LendRequests.Where(l => l.LendStatus == "Approved").Include(b => b.Book).Include(u => u.User);
            return View(await library_Management_SystemContext.ToListAsync());
        }
        public async Task<IActionResult> History()
        {
            var library_Management_SystemContext = _context.LendRequests.Where(l => l.LendStatus == "Returned" || l.LendStatus == "Declined").Include(b => b.Book).Include(u => u.User);
            return View(await library_Management_SystemContext.ToListAsync());
        }
        public IActionResult requestApproved(int lendId)
        {
            LendRequest lendRequest = _lendRepo.getLendRequestById(lendId);
            lendRequest.LendStatus = "Approved";
            lendRequest.ReturnDate = lendRequest.LendDate.AddDays(14);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult requestDeclined(int lendId)
        {
            LendRequest lendRequest = _lendRepo.getLendRequestById(lendId);
            lendRequest.LendStatus = "Declined";
            lendRequest.ReturnDate = lendRequest.LendDate;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // User
        public async Task<IActionResult> Issue()
        {
            var username = HttpContext.Session.GetString("username");
            var user = _accountsRepo.getUserByName(username);
            var library_Management_SystemContext = _context.LendRequests.Where(l => l.LendStatus.Equals("Approved") && l.UserId == user.UserId).Include(b => b.Book).Include(u => u.User);
            return View(await library_Management_SystemContext.ToListAsync());
        }

        public async Task<IActionResult> Record()
        {
            var username = HttpContext.Session.GetString("username");
            var user = _accountsRepo.getUserByName(username);
            var library_Management_SystemContext = _context.LendRequests.Where(l => l.UserId == user.UserId).Include(b => b.Book).Include(u => u.User);
            return View(await library_Management_SystemContext.ToListAsync());
        }

        public IActionResult RequestToLend(int bookId)
        {
            var username = HttpContext.Session.GetString("username");
            var user = _accountsRepo.getUserByName(username);
            var numberofCopies = _context.Books.SingleOrDefault(b => b.BookId == bookId).NumberOfCopies;
            if (numberofCopies <= 0)
            {
                _context.Books.SingleOrDefault(b => b.BookId == bookId).IsAvailable = false;
                return View("RequestedError");
            }
            var alreadyRequested = _context.LendRequests.FirstOrDefault(l => l.BookId == bookId && l.UserId == user.UserId && l.LendStatus == "Requested");
            if (alreadyRequested != null && alreadyRequested.LendStatus == "Requested")
            {
                return View("AlreadyRequestedError");
            }
            _context.Books.SingleOrDefault(b => b.BookId == bookId).NumberOfCopies--;
            LendRequest lendRequest = new LendRequest()
            {
                LendStatus = "Requested",
                LendDate = DateTime.Now,
                BookId = bookId,
                UserId = user.UserId,
                Book = _context.Books.SingleOrDefault(b => b.BookId == bookId),
                User = _context.Accounts.SingleOrDefault(u => u.UserId == user.UserId),
            };
            _context.LendRequests.Add(lendRequest);
            _context.SaveChanges();
            return View("Requested");
        }

        public IActionResult returnBook(int lendId)
        {
            var lendedBook = _context.LendRequests.FirstOrDefault(l => l.LendId == lendId);
            _context.LendRequests.Where(l => l.LendId == lendId).Include(b => b.Book).Include(u => u.User).FirstOrDefault(l => l.LendId == lendId).Book.NumberOfCopies++;
            lendedBook.LendStatus = "Returned";
            TimeSpan t = DateTime.Now - lendedBook.ReturnDate;
            lendedBook.FineAmount = t.Days - 14 > 0 ? (t.Days - 14) * 20 : 0;
            lendedBook.ReturnDate = DateTime.Now;
            _context.SaveChanges();
            return View("Return");
        }

        // GET: LendRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LendId == id);
            if (lendRequest == null)
            {
                return NotFound();
            }

            return View(lendRequest);
        }

        // GET: LendRequests/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId");
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "UserName");
            return View();
        }

        // POST: LendRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LendId,LendStatus,LendDate,ReturnDate,UserId,BookId,FineAmount")] LendRequest lendRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lendRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "UserName", lendRequest.UserId);
            return View(lendRequest);
        }

        // GET: LendRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests.FindAsync(id);
            if (lendRequest == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "UserName", lendRequest.UserId);
            return View(lendRequest);
        }

        // POST: LendRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LendId,LendStatus,LendDate,ReturnDate,UserId,BookId,FineAmount")] LendRequest lendRequest)
        {
            if (id != lendRequest.LendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lendRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LendRequestExists(lendRequest.LendId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "UserName", lendRequest.UserId);
            return View(lendRequest);
        }

        // GET: LendRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LendId == id);
            if (lendRequest == null)
            {
                return NotFound();
            }

            return View(lendRequest);
        }

        // POST: LendRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lendRequest = await _context.LendRequests.FindAsync(id);
            _context.LendRequests.Remove(lendRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LendRequestExists(int id)
        {
            return _context.LendRequests.Any(e => e.LendId == id);
        }
    }
}
