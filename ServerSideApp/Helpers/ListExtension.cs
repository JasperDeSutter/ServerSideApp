using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using ServerSideApp.Models;

namespace ServerSideApp.Helpers
{
    public static class ListExtension
    {
        public static IEnumerable<SelectListItem> CastToSelectList<T>(this IEnumerable<T> list) where T : INameId {
            return list.Select(item => new SelectListItem { Text = item.Name, Value = "" + item.Id });
        }

        public static IEnumerable<SelectListItem> CastToSelectListSelectedOn<T>(this IEnumerable<T> list, int selected) where T : INameId {
            return list.Select(item => new SelectListItem { Text = item.Name, Value = "" + item.Id, Selected = item.Id == selected });
        }

        public static string GetNameFromId<T>(this IEnumerable<T> list, int id) where T : INameId {
            return list.First(e => e.Id == id).Name;
        }

        //public static IEnumerable<TOut> Cast<TIn, TOut>(this IEnumerable<TIn> list, Func<TIn, TOut> selector) {
        //    return list.Select(selector);
        //} 
    }
}