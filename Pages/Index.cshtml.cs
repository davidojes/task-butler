﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;
using ToDoList.Resources;

namespace ToDoList
{
  [Authorize]
  public class HomeModel : PageModel
    {

    private readonly ToDoList.Data.ToDoListContext _context;
    private readonly ToDoList.Controllers.ToDoListController controller;
    

    public HomeModel(ToDoList.Data.ToDoListContext context, ToDoList.Controllers.ToDoListController controller)
    {
      _context = context;
      this.controller = controller;
    }

    public List<ToDoListItem> ToDoListItems { get; set; }
    [BindProperty]
    public ToDoListItem toDoListItem { get; set; }

    public async Task OnGetAsync()
    {
      ToDoListItems = await _context.ToDoListItem.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      _context.ToDoListItem.Add(toDoListItem);
      await _context.SaveChangesAsync();

      return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      toDoListItem = await _context.ToDoListItem.FindAsync(id);

      if (toDoListItem != null)
      {
        _context.ToDoListItem.Remove(toDoListItem);
        await _context.SaveChangesAsync();
      }

      return RedirectToPage("Index");
    }

    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostEditAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      _context.Attach(toDoListItem).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ToDoListItemExists(toDoListItem.Id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return RedirectToPage("Index");
    }

    private bool ToDoListItemExists(int id)
    {
      return _context.ToDoListItem.Any(e => e.Id == id);
    }

  }
}