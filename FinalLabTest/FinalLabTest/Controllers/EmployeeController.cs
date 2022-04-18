using FinalLabTest.DB_Context;
using FinalLabTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLabTest.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        MytableContext dbobj = new MytableContext();
        public IActionResult Index()
        {
            List<EmpRes> obj = new List<EmpRes>();

            var res = dbobj.MyInfos.ToList();
            foreach (var item in res)
            {
                obj.Add(new EmpRes
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Salary = item.Salary,
                    City = item.City,
                    Dept = item.Dept
                });
            }

            return View(obj);
        }
        [HttpGet]
        public IActionResult AddEmp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmp(EmpRes modobj)
        {
            MyInfo tblobj = new MyInfo();

            tblobj.Id = modobj.Id;
            tblobj.Name = modobj.Name;
            tblobj.Email = modobj.Email;
            tblobj.Salary = modobj.Salary;
            tblobj.City = modobj.City;
            tblobj.Dept = modobj.Dept;

            if (modobj.Id == 0)
            {
                dbobj.MyInfos.Add(tblobj);
                dbobj.SaveChanges();
            }

            else
            {
                dbobj.Entry(tblobj).State = EntityState.Modified;
                dbobj.SaveChanges();
            }

            return RedirectToAction("Index", "Employee");
        }
        public IActionResult Edit(int id)
        {
            EmpRes modobj = new EmpRes();
            var edit = dbobj.MyInfos.Where(a => a.Id == id).First();
            modobj.Id = edit.Id;
            modobj.Name = edit.Name;
            modobj.Email = edit.Email;
            modobj.Salary = edit.Salary;
            modobj.City = edit.City;
            modobj.Dept = edit.Dept;
            return View("AddEmp", modobj);
        }

        public IActionResult Delete(int id)
        {
            var res = dbobj.MyInfos.Where(a => a.Id == id).First();
            dbobj.MyInfos.Remove(res);
            dbobj.SaveChanges();
            return RedirectToAction("Index", "Employee");
        }
    }
}
