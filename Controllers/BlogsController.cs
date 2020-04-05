using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogDemo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogDemo.Controllers
{
    // only allow logged in user access
    [Authorize]
    public class BlogsController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public BlogsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Blogs
        public ActionResult Index()
        {
            // return all blogs
            return View(db.Blogs.OrderByDescending(a => a.CreatedOn).ToList());
        }

        // GET: Blogs/Manage
        public ActionResult Manage()
        {
            // return only blogs created by the user
            var currentUser = manager.FindById(User.Identity.GetUserId());
            return View(db.Blogs.OrderByDescending(a => a.CreatedOn).ToList().Where(blog => blog.User.Id == currentUser.Id));
        }

        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/BlogDetails/5
        public ActionResult BlogDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BlogTitle,BlogText,CreatedOn")] Blog blog)
        {
            // before saving, add User ID to the blog object
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                blog.User = currentUser;
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            // before allowing edit, checks user id
            var currentUser = manager.FindById(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            if (blog.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BlogTitle,BlogText,CreatedOn")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            // before allowing delete, checks user id
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            if (blog.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
