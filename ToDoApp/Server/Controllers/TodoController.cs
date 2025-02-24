﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.ApplicationLayer.Services.Contracts;
using ToDoApp.Shared;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.Utils;

namespace ToDoApp.Server.Controllers;

[Authorize]
[ApiController]
public class TodoController : Controller
{
    private readonly ITodoService _toDoService;

    public TodoController(ITodoService toDoService)
    {
        _toDoService = toDoService;
    }

    [HttpGet]
    [Route(ApiEndpoints.TodoEndpoints.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _toDoService.GetAll());
    }

    [HttpPost]
    [Route(ApiEndpoints.TodoEndpoints.GetPaginatedResult)]
    public async Task<IActionResult> GetPaginatedResult(PageRequest pageRequest)
    {
        var userProfileId = User.GetUserProfileId();

        try
        {
            var result = await _toDoService.GetPaginatedResult(pageRequest, userProfileId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Ok();
        }
    }

    [HttpGet]
    [Route(ApiEndpoints.TodoEndpoints.GetById)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await _toDoService.GetById(id);

        if (result is not null)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpDelete]
    [Route(ApiEndpoints.TodoEndpoints.Delete)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _toDoService.Delete(id);

        if (result is not null)
        {
            return Ok(result);
        }

        return NotFound();
    }

    [HttpPost]
    [Route(ApiEndpoints.TodoEndpoints.Save)]
    public async Task<IActionResult> Save(TodoModel model)
    {
        var userProfileId = User.GetUserProfileId();
        model.UserProfileId = userProfileId;
        return Ok(await _toDoService.Save(model));
    }
}