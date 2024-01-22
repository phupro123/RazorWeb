using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EF.models;

namespace newEF.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly EF.models.MyBlogContext _context;

        public IndexModel(EF.models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get; set; } = default!;

        public const int ITEMS_PER_PAGE = 20;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }

        public int countPages { get; set; }

        public async Task OnGetAsync(string SearchString)
        {
            if (_context.articles != null)
            {
                // Article = await _context.articles.ToListAsync();

                int totalArticle = await _context.articles.CountAsync();
                Console.WriteLine(totalArticle);
                countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);

                if (currentPage < 1)
                    currentPage = 1;

                if (currentPage > countPages)
                    currentPage = countPages;



                var qr = (from c in _context.articles
                          orderby c.Created descending
                          select c).Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE);
                if (!string.IsNullOrEmpty(SearchString))
                {
                    Console.WriteLine(SearchString);
                    // Truy vấn lọc các Article mà tiêu đề chứa chuỗi tìm kiếm
                    Article = qr.Where(article => article.Title.Contains(SearchString)).ToList();
                }
                else
                {
                    Article = await qr.ToListAsync();
                }


            }
        }
    }
}
