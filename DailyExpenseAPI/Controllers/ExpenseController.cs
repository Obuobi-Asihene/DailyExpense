using DailyExpenseAPI.Models;
using DailyExpenseAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DailyExpenseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        //get expense by id
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        //get all expenses
        [HttpGet("GetAllExpenses")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            var expenses = await _expenseService.GetAllExpnsesAsync();
            return Ok(expenses);
        }

        //add expense
        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense(Expense expense)
        {
            await _expenseService.AddExpenseAsync(expense);
            return Ok("Expense created succesfully");
        }

        //update expense
        [HttpPut("UpdateExpense/{id}")]
        public async Task<ActionResult> UpdateExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
            }
            try
            {
                await _expenseService.UpdateExpenseAsync(expense);
            }
            catch (Exception)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool ExpenseExists(int id)
        {
            return true;
        }

        //delete expense
        [HttpDelete("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            await _expenseService.DeleteExpenseAsync(id);
            return Ok("Expense Deleted");
        }
    }
}
