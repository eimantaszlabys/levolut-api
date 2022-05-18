using Levolut.Api.V2.Contracts;
using Levolut.Api.V2.Infrastructure.Database;
using Levolut.Api.V2.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Mvc;
// What is the responsibility of a controller?
// What code should it contain and should this be here?
using Microsoft.EntityFrameworkCore;

namespace Levolut.Api.V2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAdminController : ControllerBase
    {
        private readonly IBankFeeService bankFeeSerivce;

        public BankAdminController(IBankFeeService bankFeeSerivce)
        {
            this.bankFeeSerivce = bankFeeSerivce;
        }

        [HttpGet(Name = "Rule/{bankId}")]
        public IActionResult GetRules(long bankId)
        {
            // Mention that it could be improved by having 3 different kinds of models:
            // Db, Domain, Api.

            // What are the alternatives to services?
            // Services, mediator, use case object.
            var bankFeeRule = bankFeeSerivce.GetBankFeeRule(bankId);

            return Ok(new GetBankFeeRulesResponse
            {
                BankFeeRule = bankFeeRule,
            });
        }

        [HttpPost(Name = "Rule/{bankId}")]
        public IActionResult AddRule(long bankId, AddBankFeeRuleRequest request)
        {
            // We could call query handler directly, because that's all it does NOW.
            // Should we allow ourselves to do that?

            // This is a code smell because it does a little bit too much already.
            // We should use automapper or similar.
            var bankFeeRule = new BankFeeRule
            {
                BankId = bankId,
                Fee = request.Fee,
                BlockedCountries = request.BlockedCountries.Select(bc => new BlockedCountry { Name = bc }).ToList()
            };
            var entity = bankFeeSerivce.AddBankFeeRule(bankFeeRule);
            return Ok(entity);
        }
    }
}