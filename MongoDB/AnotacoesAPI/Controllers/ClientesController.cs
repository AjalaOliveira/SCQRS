using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Models;
using System;
using System.Threading.Tasks;

namespace MongoDB.Controllers
{
    public class ClientesController : Controller
    {
        private readonly SQLDbContext _sQLDbContext;

        public ClientesController(SQLDbContext sQLDbContext)
        {
            _sQLDbContext = sQLDbContext;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _sQLDbContext.Clientes.ToListAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cliente cliente = await _sQLDbContext.Clientes.FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Cliente cliente)
        {
            if (CPFValidation.Validate(cliente.CPF) && EmailValidation.Validate(cliente.Email))
            {

                if (ModelState.IsValid)
                {
                    Cliente existeCpf = await _sQLDbContext.Clientes.FirstOrDefaultAsync(m => m.CPF == cliente.CPF);
                    Cliente existeEmail = await _sQLDbContext.Clientes.FirstOrDefaultAsync(m => m.Email == cliente.Email);

                    if (existeCpf == null && existeEmail == null)
                    {
                        _sQLDbContext.Add(cliente);
                        await _sQLDbContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction("Add", "Clientes");
                    }
                }
            }
            return View(cliente);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = _sQLDbContext.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

    }
}
