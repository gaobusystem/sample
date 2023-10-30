using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyBlogApp.Models;

namespace MyBlogApp.Controllers
{
    [CategoryFilter]
    public class ArticlesController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Articles
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Articles.OrderByDescending(item => item.Modified).ToList());

         //   var blogds = from s in db.Articles
         //               orderby s.Modified descending, s.Title
         //               select s;
         //   return View(blogds);
         //   return View(db.Articles.ToList());
        }

        // GET: Articles/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "Owners")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]
        public ActionResult Create([Bind(Include = "Id,Title,Body,CategoryName")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.Created = DateTime.Now;
                article.Modified = DateTime.Now;

                // DBからカテゴリーを取得
                var category = db.Categories
                    .Where(item => item.CategoryName.Equals(article.CategoryName))
                    .FirstOrDefault();
                if (category == null)
                {
                    // カテゴリーを登録
                    category = new Category()
                    {
                        CategoryName = article.CategoryName,
                        Count = 1
                    };
                    db.Categories.Add(category);
                }
                else
                {
                    // カテゴリーを更新
                    category.Count++;
                    db.Entry(category).State = EntityState.Modified;
                }

                article.Category = category;

                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize(Roles = "Owners")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            article.CategoryName = article.Category.CategoryName;

            return View(article);
        }

        // POST: Articles/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,CategoryName")] Article article)
        {
            if (ModelState.IsValid)
            {
                // DBから取得
                var dbArticle = db.Articles.Find(article.Id);
                if (dbArticle == null)
                {
                    return HttpNotFound();
                }

                // 更新内容を反映
                dbArticle.Title = article.Title;
                dbArticle.Body = article.Body;
                dbArticle.Modified = DateTime.Now;
                dbArticle.CategoryName = article.CategoryName;

                // カテゴリー変更
                var beforeCategory = dbArticle.Category;
                if (!beforeCategory.CategoryName.Equals(article.CategoryName))
                {
                    // 前カテゴリーを更新
                    beforeCategory.Articles.Remove(dbArticle);
                    beforeCategory.Count--;
                    db.Entry(beforeCategory).State = EntityState.Modified;

                    // 新カテゴリーを取得
                    var category = db.Categories
                        .Where(item => item.CategoryName.Equals(article.CategoryName))
                        .FirstOrDefault();

                    if (category == null)
                    {
                        // 新カテゴリーを追加
                        category = new Category()
                        {
                            CategoryName = article.CategoryName,
                            Count = 1
                        };
                        db.Categories.Add(category);
                    }
                    else
                    {
                        // 新カテゴリーを更新
                        category.Count++;
                        db.Entry(category).State = EntityState.Modified;
                    }

                    // 新カテゴリーを記事と紐づけ
                    dbArticle.Category = category;
                }

                db.Entry(dbArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "Owners")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);

            // カテゴリーの件数を-1
            Category category = article.Category;
            category.Count--;
            db.Entry(category).State = EntityState.Modified;

            // コメントを削除
            article.Comments.Clear();
            
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Articles/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateComment([Bind(Include = "ArticleId,Body")] Comment comment)
        {
            var article = db.Articles.Find(comment.ArticleId);
            if (article == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            comment.Created = DateTime.Now;
            comment.Article = article;

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = comment.ArticleId });
        }

        // GET: Articles/DeleteComment/5
        [Authorize(Roles = "Owners")]
        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Articles/DeleteComment/5
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owners")]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            var comment = db.Comments.Find(id);
            int articleId = comment.Article.Id;

            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = articleId });
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
