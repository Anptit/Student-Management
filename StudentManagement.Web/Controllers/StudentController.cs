using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Interfaces.Services;
using StudentManagement.Core.ViewModel;

namespace StudentManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string search = "", string classSearch = "" ,int pageSize = 0, int pageIndex = -1)
        {
            try
            {
                var students = await _studentService.GetAll(search, classSearch , pageSize, pageIndex);
                return Ok(students);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel viewModel)
        {
            try
            {
                return Ok(await _studentService.Create(viewModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> Detail(int studentId)
        {
            try
            {
                var student = await _studentService.Get(studentId);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{studentId}")]
        public async Task<IActionResult> Update(int studentId, StudentViewModel viewModel)
        {
            try
            {
                return Ok(await _studentService.Update(studentId, viewModel));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> Delete(int studentId)
        {
            try
            {
                return Ok(await _studentService.Delete(studentId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
    }
}
