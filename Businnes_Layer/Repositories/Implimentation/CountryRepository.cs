using AutoMapper;
using Businnes_Layer.Repositories.Interfaces;
using Data_Layer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Api_Core_.Model;

namespace Businnes_Layer.Repositories.Implimentation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _context;
        
        public CountryRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }


        public bool countryExists(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetAllCountries()
        {
            return _context.Countries.ToList();

        }

        public Country GetCountryById(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId)
                .Select( c => c.Country ).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
          return _context.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
