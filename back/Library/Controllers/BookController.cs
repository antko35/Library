/*using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Library.Application.Contracts.Book;
using Library.Application.Use_Cases.Books;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Books")]
    public class BookController : ControllerBase
    {
       // private readonly IBookService _bookServise;
        private readonly IValidator<RequestBookDto> _validator;

        private readonly GetAllBooksUseCase _getAllBooksUseCase;
        private readonly GetBooksByPageUseCase _getBooksByPageUseCase;
        private readonly GetBookByIdUseCase _getBookByIdUseCase;
        private readonly GetBookByIsbnUseCase _getBookByIsbnUseCase;
        private readonly GetBooksCountUseCase _getBookCountUseCase;
        private readonly CreateBookUseCase _createBookUseCase;
        private readonly UpdateBookUseCase _updateBookUseCase;
        private readonly BorrowBookUseCase _borrowBookUseCase;
        private readonly ReturnBookUseCase _returnBookUseCase;
        private readonly UploadCoverUseCase _uploadCoverUseCase;
        private readonly DeleteBookUseCase _deleteBookUseCase;
        public BookController(
           // IBookService service,
            IValidator<RequestBookDto> validator,
            GetAllBooksUseCase getAllBooksUseCase,
            GetBooksByPageUseCase getBooksByPageUseCase,
            GetBookByIdUseCase getBookByIdUseCase,
            GetBookByIsbnUseCase getBookByIsbnUseCase,
            GetBooksCountUseCase getBooksCountUseCase,
            CreateBookUseCase createBookUseCase,
            UpdateBookUseCase updateBookUseCase,
            DeleteBookUseCase deleteBookUseCase,
            BorrowBookUseCase borrowBookUseCase,
            ReturnBookUseCase returnBookUseCase,
            UploadCoverUseCase uploadCoverUseCase
            )
        {
            //_bookServise = service;
            _validator = validator;
            _getAllBooksUseCase = getAllBooksUseCase;
            _getBooksByPageUseCase = getBooksByPageUseCase;
            _getBookByIdUseCase = getBookByIdUseCase;
            _getBookByIsbnUseCase = getBookByIsbnUseCase;
            _getBookCountUseCase = getBooksCountUseCase;
            _createBookUseCase = createBookUseCase;
            _updateBookUseCase = updateBookUseCase;
            _borrowBookUseCase = borrowBookUseCase;
            _returnBookUseCase = returnBookUseCase;
            _deleteBookUseCase = deleteBookUseCase;
            _uploadCoverUseCase = uploadCoverUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseBookDto>>> GetAll()
        {
            var books = await _getAllBooksUseCase.Execute();
            return Ok(books);
        }

        [HttpGet]
        [Route("getBypage/{page:int}/{pageSize:int}")]
        public async Task<ActionResult<List<ResponseBookDto>>> GetPage([FromRoute] int page, int pageSize)
        {
            var books = await _getBooksByPageUseCase.Execute(page, pageSize);
            return Ok(books);
        }

        [HttpGet]
        [Route("getById/{Id:Guid}")]
        public async Task<ActionResult<ResponseBookDto>> GetById([FromRoute] Guid Id)
        {
            try
            {
                var book = await _getBookByIdUseCase.Execute(Id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("getByIsbn/{isbn}")]
        public async Task<ActionResult<ResponseBookDto>> GetByIsbn([FromRoute] string isbn)
        {
            try
            {
                var book = await _getBookByIsbnUseCase.Execute(isbn);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> GetBooksCount()
        {
            var cont = await _getBookCountUseCase.Execute();
            return Ok(cont);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<ResponseBookDto>> Create([FromBody] RequestBookDto createBookDto)
        {
            ValidationResult result = await _validator.ValidateAsync(createBookDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var createdBook = await _createBookUseCase.Execute(createBookDto);
                return Ok(createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<ResponseBookDto>> Update([FromBody] RequestUpdateBookDto requestBookDto)
        {
            try
            {
                var book = await _updateBookUseCase.Execute(requestBookDto);
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("borrow/{bookId:Guid}")]
        public async Task<ActionResult> BorrowBook([FromRoute] Guid bookId)
        {
            try
            {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);
                await _borrowBookUseCase.Execute(bookId, UserId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return/{bookId:Guid}")]
        public async Task<ActionResult> ReturnBook([FromRoute] Guid bookId)
        {
            try
            {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);

                await _returnBookUseCase.Execute(bookId, UserId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("upload-cover/{bookId}")]
        public async Task<ActionResult> UploadCover(Guid bookId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Invalid file.");

                await _uploadCoverUseCase.Execute(bookId, file);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _deleteBookUseCase.Execute(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Library.Application.Contracts.Book;
using Library.Application.Use_Cases.Books;

namespace Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("Books")]
    public class BookController : ControllerBase
    {
        private readonly IValidator<RequestBookDto> _validator;

        private readonly GetAllBooksUseCase _getAllBooksUseCase;
        private readonly GetBooksByPageUseCase _getBooksByPageUseCase;
        private readonly GetBookByIdUseCase _getBookByIdUseCase;
        private readonly GetBookByIsbnUseCase _getBookByIsbnUseCase;
        private readonly GetBooksCountUseCase _getBookCountUseCase;
        private readonly CreateBookUseCase _createBookUseCase;
        private readonly UpdateBookUseCase _updateBookUseCase;
        private readonly BorrowBookUseCase _borrowBookUseCase;
        private readonly ReturnBookUseCase _returnBookUseCase;
        private readonly UploadCoverUseCase _uploadCoverUseCase;
        private readonly DeleteBookUseCase _deleteBookUseCase;
        public BookController(
            IValidator<RequestBookDto> validator,
            GetAllBooksUseCase getAllBooksUseCase,
            GetBooksByPageUseCase getBooksByPageUseCase,
            GetBookByIdUseCase getBookByIdUseCase,
            GetBookByIsbnUseCase getBookByIsbnUseCase,
            GetBooksCountUseCase getBooksCountUseCase,
            CreateBookUseCase createBookUseCase,
            UpdateBookUseCase updateBookUseCase,
            DeleteBookUseCase deleteBookUseCase,
            BorrowBookUseCase borrowBookUseCase,
            ReturnBookUseCase returnBookUseCase,
            UploadCoverUseCase uploadCoverUseCase
            )
        {
            _validator = validator;
            _getAllBooksUseCase = getAllBooksUseCase;
            _getBooksByPageUseCase = getBooksByPageUseCase;
            _getBookByIdUseCase = getBookByIdUseCase;
            _getBookByIsbnUseCase = getBookByIsbnUseCase;
            _getBookCountUseCase = getBooksCountUseCase;
            _createBookUseCase = createBookUseCase;
            _updateBookUseCase = updateBookUseCase;
            _borrowBookUseCase = borrowBookUseCase;
            _returnBookUseCase = returnBookUseCase;
            _deleteBookUseCase = deleteBookUseCase;
            _uploadCoverUseCase = uploadCoverUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseBookDto>>> GetAll()
        {
            var books = await _getAllBooksUseCase.Execute();
            return Ok(books);
        }

        [HttpGet]
        [Route("getBypage/{page:int}/{pageSize:int}")]
        public async Task<ActionResult<List<ResponseBookDto>>> GetPage([FromRoute] int page, int pageSize)
        {
            var books = await _getBooksByPageUseCase.Execute(page, pageSize);
            return Ok(books);
        }

        [HttpGet]
        [Route("getById/{Id:Guid}")]
        public async Task<ActionResult<ResponseBookDto>> GetById([FromRoute] Guid Id)
        {

            var book = await _getBookByIdUseCase.Execute(Id);
            return Ok(book);
        }
        [HttpGet]
        [Route("getByIsbn/{isbn}")]
        public async Task<ActionResult<ResponseBookDto>> GetByIsbn([FromRoute] string isbn)
        {

            var book = await _getBookByIsbnUseCase.Execute(isbn);
            return Ok(book);
        }

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> GetBooksCount()
        {
            var cont = await _getBookCountUseCase.Execute();
            return Ok(cont);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<ResponseBookDto>> Create([FromBody] RequestBookDto createBookDto)
        {
            ValidationResult result = await _validator.ValidateAsync(createBookDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }


            var createdBook = await _createBookUseCase.Execute(createBookDto);
            return Ok(createdBook);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("update")]
        public async Task<ActionResult<ResponseBookDto>> Update([FromBody] RequestUpdateBookDto requestBookDto)
        {

            var book = await _updateBookUseCase.Execute(requestBookDto);
            return Ok(book);
        }

        [HttpPost("borrow/{bookId:Guid}")]
        public async Task<ActionResult> BorrowBook([FromRoute] Guid bookId)
        {

            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            Guid UserId = Guid.Parse(userId);
            await _borrowBookUseCase.Execute(bookId, UserId);
            return Ok();
        }

        [HttpPost("return/{bookId:Guid}")]
        public async Task<ActionResult> ReturnBook([FromRoute] Guid bookId)
        {

            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            Guid UserId = Guid.Parse(userId);

            await _returnBookUseCase.Execute(bookId, UserId);
            return Ok();
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("upload-cover/{bookId}")]
        public async Task<ActionResult> UploadCover(Guid bookId, IFormFile file)
        {

            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            await _uploadCoverUseCase.Execute(bookId, file);
            return Ok();

        }


        [Authorize(Policy = "Admin")]
        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {

            await _deleteBookUseCase.Execute(id);
            return Ok();
        }
    }
}
