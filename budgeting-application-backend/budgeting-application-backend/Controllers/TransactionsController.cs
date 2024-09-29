using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using budgeting_application.DataAccessLayer.Repository;
using budgeting_application.DataAccessLayer.Entities;
using System.Net;

namespace budgeting_application_backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        [Route("GetAllTransaction")]
        public IActionResult GetAllTransaction()
        {
            var transactions = _transactionRepository.GetAllTransaction();
            return Ok(transactions);
        }

        [HttpPost]
        [Route("AddTransaction")]
        public HttpResponseMessage AddTransaction([FromBody] Transaction transaction)
        {
            try
            {
                transaction.Date = DateTime.Now;
                _transactionRepository.AddTransaction(transaction);

                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Transaction added successfully.")
                };
                return response;
            }
            catch (InvalidOperationException ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(ex.Message)
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
                return response;
            }
        }

        //[HttpPut("{id}")]
        //[Route("UpdateTransaction/{id}")]
        //public HttpResponseMessage UpdateTransaction(int id, [FromBody] Transaction transaction)
        //{
        //    try
        //    {
        //        transaction.TransactionID = id;
        //        _transactionRepository.UpdateTransaction(transaction);

        //        var response = new HttpResponseMessage(HttpStatusCode.NoContent)
        //        {
        //            Content = new StringContent("Transaction updated successfully.")
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new StringContent($"An error occurred: {ex.Message}")
        //        };
        //        return response;
        //    }
        //}

        //[HttpDelete("{id}")]
        //[Route("DeleteTransaction/{id}")]
        //public HttpResponseMessage DeleteTransaction(int id)
        //{
        //    try
        //    {
        //        _transactionRepository.DeleteTransaction(id);

        //        var response = new HttpResponseMessage(HttpStatusCode.NoContent)
        //        {
        //            Content = new StringContent("Transaction deleted successfully.")
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new StringContent($"An error occurred: {ex.Message}")
        //        };
        //        return response;
        //    }
        //}

        [HttpGet("{id}")]
        [Route("GetTransactionById/{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = _transactionRepository.GetTransactionById(id);
            return Ok(transaction);
        }
    }
}
