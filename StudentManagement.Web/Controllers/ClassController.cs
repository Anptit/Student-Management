using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.ViewModel;

namespace StudentManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;   
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search = null, int pageSize = 0, int pageIndex = 0)
        {
            try
            {
                var classes = await _classService.GetAll(search, pageSize, pageIndex);
                return Ok(classes);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClassViewModel request)
        {
            try
            {
                return Ok(await _classService.Create(request));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var getClass = await _classService.Get(id);
                return Ok(getClass);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ClassViewModel viewModel)
        {
            try
            {
                return Ok(await _classService.Update(viewModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _classService.Delete(id));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
