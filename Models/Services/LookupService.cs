using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HireMeApp.Data;
using HireMeApp.Models;
using HireMeApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using System.Web;

namespace HireMeApp.Models.Services
{
    public class LookupService
    {
        private readonly HireMeContext _context;
        public LookupService(HireMeContext context)
        {
            _context = context;
        }

        public IQueryable<List<string>> GetCSharpList(int lid)
        {
            var namesVar = (from i in _context.InfoName
                where i.LanguageId == lid
                select new List<string> { i.Info_Name, i.Id.ToString() });

            return namesVar;
        }

        public string GetLangName(int id)
        {
            var namesVar = _context.Language.Where(s => s.Id == id).Select(g => g.LanguageName).First();
            return namesVar;
        }

        public string GetInfoName(int id)
        {
            var namesVar = _context.InfoName.Where(s => s.Id == id).Select(g => g.Info_Name).First();
            return namesVar;
        }

        public string GetLangNameTextBlock(int id)
        {
            int textBlock = _context.TextBlock.Where(s => s.Id == id).Select(g => g.InfoId).First();

            int infoName = _context.InfoName.Where(s => s.Id == textBlock).Select(g => g.LanguageId).First();

            var namesVar = _context.Language.Where(s => s.Id == infoName).Select(g => g.LanguageName).First();

            return namesVar;
        }
    }
}
