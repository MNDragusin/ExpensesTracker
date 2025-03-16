using AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace MauiClient.PageModels
{
    public class ShellViewModel{
        
        private readonly DataContext _dbContext; 
        public ShellViewModel(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        public List<string> GetWalletsNames(){
            return _dbContext.Wallets.Select(x => x.Name).ToList();
        }
    }
}