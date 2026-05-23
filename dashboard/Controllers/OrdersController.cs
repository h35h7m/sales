using Dashboard.Application.Interfaces;
using Dashboard.Application.ViewModels;
using Dashboard.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(OrderSearchViewModel searchModel)
    {
        var model = await _service.GetOrdersAsync(searchModel);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.ProductsList = await _service.GetAllProductsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Create(Order order) 
    {
        
        if (order == null || order.OrderDetails == null || !order.OrderDetails.Any())
        {
            ModelState.AddModelError("", "add one at least");
        }

        if (ModelState.IsValid)
        {
   
            await _service.CreateOrderAsync(order);

            return RedirectToAction(nameof(Index));
        }

 
        ViewBag.ProductsList = await _service.GetAllProductsAsync();
        return View(order);
    }

    [HttpGet]
public async Task<IActionResult> Edit(int id)
{

    var order = await _service.GetOrderWithDetailsAsync(id);
    if (order == null) return NotFound();

    ViewBag.ProductsList = await _service.GetAllProductsAsync();
    return View(order);
}

[HttpPost]
[ValidateAntiForgeryToken] 
public async Task<IActionResult> Edit(Order order) 
{

    if (order == null || order.OrderDetails == null || !order.OrderDetails.Any())
    {
        ModelState.AddModelError("", "add one at least.");
    }

    if (ModelState.IsValid)
    {

        await _service.UpdateOrderAsync(order);

        return RedirectToAction(nameof(Index));
    }

    ViewBag.ProductsList = await _service.GetAllProductsAsync();
    return View(order);
}

    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteOrderAsync(id);
        return RedirectToAction(nameof(Index));
    }
}