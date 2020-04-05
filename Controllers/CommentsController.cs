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
    [Authorize]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public CommentsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Comments
        //public ActionResult Index()
        //{
        //    return View(db.Comments.ToList());
        //}

        // GET: Comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comment comment = db.Comments.Find(id);
        //    if (comment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comment);
        //}

        // GET: Comments/Create
        public ActionResult Create(int? id)
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
            ViewBag.BlogTitle = blog.BlogTitle;
            ViewBag.BlogId = blog.Id;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id, [Bind(Include = "Id,CommentText,CreatedOn")] Comment comment)
        {
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
            if (ModelState.IsValid)
            {
                comment.User = currentUser;
                comment.Blog = blog;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("BlogDetails/" + id, "Blogs");
            }

            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (comment.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            Blog blog = db.Blogs.Find(comment.BlogId);
            ViewBag.BlogTitle = blog.BlogTitle;
            ViewBag.BlogId = blog.Id;
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? d, [Bind(Include = "Id,CommentText,CreatedOn,BlogId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BlogDetails/" + comment.BlogId, "Blogs");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            if (comment.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            Blog blog = db.Blogs.Find(comment.BlogId);
            ViewBag.BlogTitle = blog.BlogTitle;
            ViewBag.BlogId = blog.Id;
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            int bid = comment.BlogId;
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("BlogDetails/" + bid, "Blogs");
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
