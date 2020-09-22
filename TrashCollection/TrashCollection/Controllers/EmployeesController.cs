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

        //GET: Select from dropdown
        [HttpPost]
        public async Task<IActionResult> Select(CustomersDayFilter cdf)
        {
            cdf = new CustomersDayFilter();

            var applicationDbContext = _context.Employee.Include(e => e.IdentityUser);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInEmployee = _context.Employee.Where(e => e.IdentityUserId == userId).SingleOrDefault();

            var customerInZipcode = _context.customers.Where(c => c.ZipCode == loggedInEmployee.ZipCode).ToList();

            //var customerPickUpDay = cdf.Customers;

            //cdf.Customers = customerInZipcode.Where(c => c.Day.ToString() == customerPickUpDay).ToList();

            //var loggedInEmployee2 = _context.Employee.Where(c => c.IdentityUserId == userId).Include(c => c.IdentityUser);

            //ViewData["EmployeeExists"] = loggedInEmployee2.Count() == 1;

            //return View("Index", customerPickUpDay);
            return View(cdf);
        }

        //public List<Customer> GetCustomersByPickupDay(string dayToFilterBy)
        //{

        //}

        // GET: Employees/Details/5
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
            return View(employee);
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
