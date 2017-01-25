﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InstaXamarinWeb;
using InstaXamarinWeb.Models;

namespace InstaXamarinWeb.Controllers
{
    public class PostsController : Controller
    {
        private DB db = new DB();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.UsuarioDados);
            return View(posts.ToList());
        }

        public ActionResult Denunciados()
        {
            var posts = db.Posts.Include(p => p.UsuarioDados).Where(pp => pp.Denunciado == true);
            return View(posts.ToList());
        }

        public ActionResult ComentariosDenunciados()
        {
            var posts = db.PostsInteracoes.Include(p => p.UsuarioDados).Where(pp => pp.Denunciado == true);
            return View(posts.ToList());
        }


        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            //Get Denuncias
            ViewBag.Denuncias = db.PostsInteracoes.Include(p => p.UsuarioDados).Where(dd => dd.Tipo == PostInteracao.TipoInteracao.DenunciaPost && dd.PostId == post.Id).ToList();

            return View(post);
        }

        public ActionResult DetailsComentario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostInteracao postI = db.PostsInteracoes.Find(id);
            if (postI == null)
            {
                return HttpNotFound();
            }

            //Get Denuncias
            ViewBag.Denuncias = db.PostsInteracoes.Include(p => p.UsuarioDados).Where(dd => dd.Tipo == PostInteracao.TipoInteracao.DenunciaComentario && dd.ComentarioId == postI.Id).ToList();

            return View(postI);
        }


        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nome", post.UsuarioId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Data,UsuarioId,Foto,Descricao,Latitude,Longitude,QuantidadeCurtidas,QuantidadeComentarios,QuantidadeDenuncias,Denunciado,Bloqueado")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nome", post.UsuarioId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult DeleteComentario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostInteracao postI = db.PostsInteracoes.Find(id);
            db.PostsInteracoes.Remove(postI);
            db.SaveChanges();
            return RedirectToAction("ComentariosDenunciados");
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
