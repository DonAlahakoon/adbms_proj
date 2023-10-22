﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blood_Donor_Portal.Data;
using Blood_Donor_Portal.Models;
using Microsoft.Data.SqlClient;

namespace Blood_Donor_Portal.Controllers
{
    public class BloodCampsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BloodCampsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BloodCamps
        public async Task<IActionResult> Index()//Using procedure
        {
            var GetAllBloodCamps = _context.BloodCamp.FromSqlRaw("GetAllBloodCamps").ToList();
            return View(GetAllBloodCamps);
        }
        // GET: BloodCamps/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }
        // POST: BloodCamps/ShowSearchForm
        public async Task<IActionResult> ShowSearchResult(DateTime SearchDate,String SearchLocation,String SearchOrganization)
        {
            return  View("Index",await _context.BloodCamp.Where(j=>j.Date.Equals(SearchDate)||j.Location.Contains(SearchLocation)|| j.Organization.Contains(SearchOrganization)).ToListAsync());
        }

        // GET: BloodCamps/Details/5
        public async Task<IActionResult> Details(int? id)//using procedure
        {
            if (id == null || _context.BloodCamp == null)
            {
                return NotFound();
            }
            
            var bloodCamp = _context.BloodCamp.FromSqlRaw($"GetBloodCampByID {id}").AsEnumerable().FirstOrDefault();
            if (bloodCamp == null)
            {
                return NotFound();
            }

            return View(bloodCamp);
        }

        // GET: BloodCamps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BloodCamps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Camp_ID,Date,Time,Location,Coordinator,Organization")] BloodCamp bloodCamp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloodCamp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bloodCamp);
        }

        // GET: BloodCamps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BloodCamp == null)
            {
                return NotFound();
            }

            var bloodCamp = await _context.BloodCamp.FindAsync(id);
            if (bloodCamp == null)
            {
                return NotFound();
            }
            return View(bloodCamp);
        }

        // POST: BloodCamps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Camp_ID,DateTime Date,TimeSpan StartTime,TimeSpan EndTime,String Location,String Coordinator,String Organization)//using procedure
        {
            var param = new SqlParameter[]
            {
                new SqlParameter()
                {
                    ParameterName="@Id",
                    SqlDbType=System.Data.SqlDbType.Int,
                    Value=Camp_ID
                },
                new SqlParameter()
                {
                    ParameterName="@Date",
                    SqlDbType=System.Data.SqlDbType.DateTime2,
                    Value=Date
                },new SqlParameter()
                {
                    ParameterName="@STime",
                    SqlDbType=System.Data.SqlDbType.Time,
                    Value=StartTime
                },new SqlParameter()
                {
                    ParameterName="@ETime",
                    SqlDbType=System.Data.SqlDbType.Time,
                    Value=EndTime
                },new SqlParameter()
                {
                    ParameterName="@Location",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=Location
                },
                new SqlParameter()
                {
                    ParameterName="@Cordinator",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=Coordinator
                },
                new SqlParameter()
                {
                    ParameterName="@Organization",
                    SqlDbType=System.Data.SqlDbType.VarChar,
                    Value=Organization
                }
            };

            var UpdateBloodCamp = await _context.Database.ExecuteSqlRawAsync($"Exec UpdateBloodCampById @Id,@Date,@Stime,@ETime,@Location,@Cordinator,@Organization",param);
            if(UpdateBloodCamp == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(UpdateBloodCamp);
            }
            
        }

        // GET: BloodCamps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BloodCamp == null)
            {
                return NotFound();
            }

            var bloodCamp = await _context.BloodCamp
                .FirstOrDefaultAsync(m => m.Camp_ID == id);
            if (bloodCamp == null)
            {
                return NotFound();
            }

            return View(bloodCamp);
        }

        // POST: BloodCamps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BloodCamp == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BloodCamp'  is null.");
            }
            var bloodCamp = await _context.BloodCamp.FindAsync(id);
            if (bloodCamp != null)
            {
                _context.BloodCamp.Remove(bloodCamp);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodCampExists(int id)
        {
          return (_context.BloodCamp?.Any(e => e.Camp_ID == id)).GetValueOrDefault();
        }
    }
}
