﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Models
{
    public class TypeRepository
    {
        public IEnumerable<SelectListItem> GetTypes()
        {
            using (var context = new DHTDTTDNEntities1())
            {
                List<SelectListItem> types = context.Types.AsNoTracking()
                   
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.IdTy,
                            Text = n.Name
                        }).ToList();
                
                return new SelectList(types, "Value", "Text");
            }
        }
    }
}