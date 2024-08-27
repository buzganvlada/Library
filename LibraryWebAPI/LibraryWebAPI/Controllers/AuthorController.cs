using AutoMapper;
using FluentValidation;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using LibraryWebAPI.Dto.Author;
using LibraryWebAPI.Extensions;
using Libray.Core;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    namespace LibraryWebAPI.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class AuthorController : ControllerBase
        {

            private readonly ILogger<AuthorController> _logger;
            private readonly IAuthorRepository _authorRepository;
            private readonly IMapper _mapper;
            private readonly IValidator<CreateAuthorDto> _createValidator;

            public AuthorController(
                ILogger<AuthorController> logger,
                IAuthorRepository authorRepository,
                IMapper mapper,
                IValidator<CreateAuthorDto> createValidator)
            {
                _logger = logger;
                _authorRepository = authorRepository;
                _mapper = mapper;
                _createValidator = createValidator;
            }

            [HttpGet]
            public async Task<PaginatedList<AuthorDto>> Get(int page = 1, int nr = 10)
            {
                var authors = await _authorRepository.GetAuthorsAsync(page, nr);
                return _mapper.Map<PaginatedList<AuthorDto>>(authors);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(int id)
            {
                var author = await _authorRepository.GetAuthorByIdAsync(id);

                if (author == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<AuthorDto>(author));
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateAuthorDto author)
            {
                var validationResult = await _createValidator.ValidateAsync(author);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.GetErrorMessages());
                }
                var authorToCreate = _mapper.Map<Author>(author);

                var createResult = await _authorRepository.CreateAuthorAsync(authorToCreate);
                return Ok(_mapper.Map<AuthorDto>(createResult));
            }

            [HttpPut]
            public async Task<IActionResult> Update(AuthorDto author)
            {
                var authorToUpdate = _mapper.Map<Author>(author);

                try
                {
                    var updateResult = await _authorRepository.UpdateAuthorAsync(authorToUpdate);
                    return Ok(_mapper.Map<AuthorDto>(updateResult));
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
                    await _authorRepository.DeleteAuthorAsync(id);
                    return Ok();
                }
                catch (KeyNotFoundException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
