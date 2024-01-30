using AutoMapper;
using DOTNET_RPG.Data;
using DOTNET_RPG.Dtos.Character;
using DOTNET_RPG.Models;
using DOTNET_RPG.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _context = dataContext;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characters = _mapper.Map<Character>(character);

            _context.Characters.Add(characters);
            await _context.SaveChangesAsync();

            serviceResponse.Data = 
                await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var dbCharacter = 
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
                if (dbCharacter is null)
                    throw new Exception($"Character with Id {updateCharacter.Id} not found!");

                dbCharacter.Name = updateCharacter.Name;
                dbCharacter.Strength = updateCharacter.Strength;
                dbCharacter.HitPoints = updateCharacter.HitPoints;
                dbCharacter.Defense = updateCharacter.Defense;
                dbCharacter.Intelligence = updateCharacter.Intelligence;
                dbCharacter.Intelligence = updateCharacter.Intelligence;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (dbCharacter is null)
                    throw new Exception($"Character with Id {id} not found!");

                _context.Remove(dbCharacter);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
