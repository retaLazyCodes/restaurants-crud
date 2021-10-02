using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext _db;

        public SqlRestaurantData(OdeToFoodDbContext db)
        {
            _db = db;
        }
        
        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            var query = 
                from r in _db.Restaurants
                where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                orderby r.Name
                select r;
            return query; 
        }

        public Restaurant GetById(int id)
        {
            return _db.Restaurants.Find(id); 
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = _db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            _db.Restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = GetById(id);
            if (restaurant != null)
            {
                _db.Restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public int GetCountOfRestaurants()
        {
            return _db.Restaurants.Count();
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }
    }
}