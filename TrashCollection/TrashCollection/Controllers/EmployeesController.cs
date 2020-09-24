using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrashCollection.Data;
using TrashCollection.Models;
using TrashCollection.Models.ViewModels;

namespace TrashCollection.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            CustomersDayFilter cdf = new CustomersDayFilter();

            var applicationDbContext = _context.Employee.Include(e => e.IdentityUser);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInEmployee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();

            var customerInZipcode = _context.customers.Where(c => c.ZipCode == loggedInEmployee.ZipCode).ToList();

            var today = DateTime.Now.DayOfWeek.ToString();

            // string test = customerInZipcode[0].Day.ToString();

            cdf.Customers = customerInZipcode.Where(c => c.Day.ToString() == today).ToList();

            var loggedInEmployee2 = _context.Employee.Where(c => c.IdentityUserId == userId).Include(c => c.IdentityUser);

            ViewData["EmployeeExists"] = loggedInEmployee2.Count() == 1;

            return View(cdf);
        }

        //POST: Select from dropdown
        [HttpPost]
        public async Task<IActionResult> Select(CustomersDayFilter cdf)
        {
            var applicationDbContext = _context.Employee.Include(e => e.IdentityUser);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInEmployee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();

            var customerInZipcode = _context.customers.Where(c => c.ZipCode == loggedInEmployee.ZipCode).ToList();

            var customerPickUpDay = cdf.DaySelection;

            cdf.Customers = customerInZipcode.Where(c => c.Day.ToString() == customerPickUpDay).ToList();
            //to ensurelog in
            var loggedInEmployee2 = _context.Employee.Where(c => c.IdentityUserId == userId).Include(c => c.IdentityUser);

            ViewData["EmployeeExists"] = loggedInEmployee2.Count() == 1;

            return View("Index", cdf);
        }

        //public List<Customer> GetCustomersByPickupDay(string dayToFilterBy)
        //{

        //}
        public async Task<IActionResult> ViewMap(int? id)
        {
            return View();
        }
        // GET: ConfirmPickup by ID
        public async Task<IActionResult> ConfirmPickup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            //to ensurelog in

            ViewData["EmployeeExists"]  = true;

            return View(customer);
        }
        //POST: ConfirmPickup by ID
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPickup(int id, [Bind("Id,Name,ZipCode,Adress,Day,RequestOfExtraPickup,StartDate,EndDate,Balance,IdentityUserId,PickedUp")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            var today = DateTime.Now.DayOfWeek.ToString();
       
            if (customer.Day.ToString() == today)
            {
                if (customer.PickedUp == false)
                {
                    customer.PickedUp = true;
                    customer.Balance += 239.91;
                    customer.Balance = Math.Round(customer.Balance, 2, MidpointRounding.AwayFromZero);
                    //need to add suspention days
                    _context.Update(customer);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["EmployeeExists"] = true;
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", customer.IdentityUserId);
            return View("Index");
        }
        // GET: Customers/View for api map
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ZipCode,Adress,IdentityUserId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                employee.IdentityUserId = userId;

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", employee.IdentityUserId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", employee.IdentityUserId);
            //where do I want this to go?
            return View("Index", employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ZipCode,Adress,IdentityUserId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //maybe use this part
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", employee.IdentityUserId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
