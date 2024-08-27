using AutoMapper;
using FluentValidation;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using LibraryWebAPI.Dto.Book;
using LibraryWebAPI.Extensions;
using Libray.Core;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {

        private readonly ILogger<BooksController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _createValidator;

        public BooksController(ILogger<BooksController> logger,
            IBookRepository bookRepository,
            IMapper mapper,
            IValidator<CreateBookDto> createValidator)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }

        [HttpGet]
        public async Task<PaginatedList<BookDto>> Get(int page, int nr)
        {
            var books = await _bookRepository.GetBooksAsync(page, nr);
            return _mapper.Map<PaginatedList<BookDto>>(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(book));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto book)
        {
            var validationResult = await _createValidator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.GetErrorMessages());
            }

            var bookToCreate = _mapper.Map<Book>(book);

            var createResult = await _bookRepository.CreateBookAsync(bookToCreate);
            return Ok(_mapper.Map<BookDto>(createResult));
        }

        [HttpPut]
        public async Task<IActionResult> Update(BookDto book)
        {
            var bookToUpate = _mapper.Map<Book>(book);

            try
            {
                var updateResult = await _bookRepository.UpdateBookAsync(bookToUpate);
                return Ok(_mapper.Map<BookDto>(updateResult));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
