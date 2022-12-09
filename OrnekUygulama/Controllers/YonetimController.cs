using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrnekUygulama.Models;

namespace OrnekUygulama.Controllers
{
    public class YonetimController : Controller
    {
        YemektarifleriDbContext db = new YemektarifleriDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Kullanicilar()
        {
            return View();
        }
        public IActionResult Bilgilerim()
        {
            return View();
        }

        //                                              <!-                            Sayfalar START                             -!>
        //                                              <!-                            Sayfalar START                             -!>

        public IActionResult Sayfalar()
        {
            var sayfalar = db.Sayfalars.Where(s => s.Silindi == false).OrderBy(s=>s.Baslik).ToList();

            return View(sayfalar);
        }
        public IActionResult SayfaEkle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SayfaEkle(Sayfalar s)
        {
            s.Silindi = false;
            db.Sayfalars.Add(s);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }
        public IActionResult SayfaGetir(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();

            return View("SayfaGuncelle",sayfa);
        }
        public IActionResult SayfaGuncelle(Sayfalar syf)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == syf.SayfaId).FirstOrDefault();
            sayfa.Baslik = syf.Baslik;
            sayfa.Icerik = syf.Icerik;
            sayfa.Aktif = syf.Aktif;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }
        public IActionResult SayfaSil(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();
            sayfa.Silindi = true;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }

        //                                              <!-                            Sayfalar END                             -!>


        //                                              <!-                            Kategoriler START                             -!>

        public IActionResult Kategoriler()
        {
            var kategoriler = db.Kategorilers.Where(k => k.Silindi == false).OrderBy(k => k.KategoriId).ToList();

            return View(kategoriler);
        }
        public IActionResult KategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult KategoriEkle(Kategoriler k)
        {
            k.Silindi = false;
            db.Kategorilers.Add(k);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public IActionResult KategoriGetir(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();

            return View("KategoriGuncelle", kategori);
        }
        public IActionResult KategoriYemekler(int id)
        {
            var yemekler = db.YemekTarifleris.Include(k=>k.Kategori).Where(y => y.Silindi == false && y.KategoriId == id).ToList();
            return View("Tarifler", yemekler);
        }
        public IActionResult KategoriGuncelle(Kategoriler ktgr)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == ktgr.KategoriId).FirstOrDefault();
            kategori.Kategoriadi = ktgr.Kategoriadi;
            kategori.Aktif = ktgr.Aktif;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }
        public IActionResult KategoriSil(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();
            kategori.Silindi = true;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        //                                              <!-                            Kategoriler END                             -!>


        //                                              <!-                            Tarifler START                             -!>

        public IActionResult Tarifler()
        {
            var tarifler = db.YemekTarifleris.Include(k=>k.Kategori).Where(t => t.Silindi == false).OrderBy(t => t.TarifId).ToList();

            return View(tarifler);
        }
        public IActionResult TarifEkle()
        {
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                select new SelectListItem
                {
                    Text = k.Kategoriadi,
                    Value = k.KategoriId.ToString()
                }
            );
            ViewBag.KategoriId = kategoriler;

            return View();
        }
        [HttpPost]
        public IActionResult TarifEkle(YemekTarifleri t)
        {
            t.Silindi = false;
            t.Eklemetarihi = DateTime.Now;
            db.YemekTarifleris.Add(t);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }
        public IActionResult TarifGetir(int id)
        {
            var tarif = db.YemekTarifleris.Include(k=>k.Kategori).Where(t => t.Silindi == false && t.TarifId == id).FirstOrDefault();
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                               select new SelectListItem
                               {
                                   Text = k.Kategoriadi,
                                   Value = k.KategoriId.ToString()
                               }
            );
            ViewBag.KategoriId = kategoriler;
            return View("TarifGuncelle", tarif);
        }
        public IActionResult TarifYorumlari(int id)
        {
            var yorumlar = db.Yorumlars.Where(y => y.Silindi == false && y.TarifId == id).ToList();
            return View("Yorumlar", yorumlar);
        }
        public IActionResult TarifGuncelle(YemekTarifleri trf)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.KategoriId == trf.KategoriId).FirstOrDefault();
            tarif.Yemekadi = trf.Yemekadi;
            tarif.Tarif = trf.Tarif;
            tarif.Sira = trf.Sira;
            tarif.KategoriId = trf.KategoriId;
            tarif.Aktif = trf.Aktif;
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }
        public IActionResult TarifSil(int id)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.KategoriId == id).FirstOrDefault();
            tarif.Silindi = true;
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        //                                              <!-                            Tarifler END                             -!>
        //                                              <!-                            Yorumlar START                           -!>

        [HttpGet]
        public IActionResult Yorumlar()
        {
            var yorumlar = db.Yorumlars.Include(t=>t.TarifNavigation).Include(u=>u.Uye).Where(y => y.Silindi == false).OrderByDescending(y => y.Eklemetarihi).ToList();

            return View(yorumlar);
        }
        [HttpPost]
        public IActionResult Yorumlar(string listelemeturu)
        {
            var yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false).OrderByDescending(y => y.Eklemetarihi).ToList();

            switch (listelemeturu)
            {
                case "Onayli": yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == true).OrderByDescending(y => y.Eklemetarihi).ToList(); break;
                case "Onaysiz": yorumlar = db.Yorumlars.Include(t=>t.TarifNavigation).Include(u=>u.Uye).Where(y => y.Silindi == false && y.Aktif == false).OrderByDescending(y => y.Eklemetarihi).ToList(); break;
            }

            return View(yorumlar);
        }
        public IActionResult Onayla(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Aktif = Convert.ToBoolean(-1*Convert.ToInt32(yorum.Aktif)+1);
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }
        public IActionResult YorumSil(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Silindi = true;
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }

        //                                              <!-                            Yorumlar END                             -!>
        //                                              <!-                            Yorumlar END                             -!>

        public IActionResult CikisYap()
        {
            return View();
        }
    }
}
