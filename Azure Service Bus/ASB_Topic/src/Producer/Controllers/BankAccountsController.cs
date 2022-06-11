using Microsoft.AspNetCore.Mvc;
using Producer.Models.Requests;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;
        public BankAccountsController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost("Deposit")]
        public async Task<ActionResult<DepositFunds>> DepositFunds([FromBody] DepositFundsDto item, CancellationToken cancellationToken)
        {
            DepositFunds funds = new()
            {
                AccountId = item.AccountId,
                Name = item.Name,
                Amount = item.Amount,
                TransactionId = Guid.NewGuid().ToString("N"),
                TransactionDate = DateTime.UtcNow
            };

            await _messagePublisher.PublishMessageAsync(funds, cancellationToken: cancellationToken).ConfigureAwait(false);
            return Accepted(funds);
        }

        [HttpPost("Withdraw")]
        public async Task<ActionResult<DepositFunds>> DepositFunds([FromBody] WithdrawFundsDto item, CancellationToken cancellationToken)
        {
            WithdrawFunds funds = new()
            {
                AccountId = item.AccountId,
                Name = item.Name,
                Amount = item.Amount,
                TransactionId = Guid.NewGuid().ToString("N"),
                TransactionDate = DateTime.UtcNow
            };

            await _messagePublisher.PublishMessageAsync(funds, cancellationToken: cancellationToken).ConfigureAwait(false);
            return Accepted(funds);
        }
    }
}
