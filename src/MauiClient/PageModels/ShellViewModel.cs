using AppDataContext;
using Microsoft.EntityFrameworkCore;

namespace MauiClient.PageModels
{
    public class ShellViewModel{
        
        private readonly DataContext _dbContext; 
        public ShellViewModel(DataContext dataContext)
        {
            _dbContext = dataContext;
            //Task.Run(Rename);
        }

        public List<string> GetWalletsNames(){
            return _dbContext.Wallets.Select(x => x.Name).ToList();
        }

        private async Task Rename()
        {
            var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(p => p.Name == "##DEV_TEST##");
            if(wallet is null)
            {
                return;
            }

            wallet.Name = "DEV_TEST";

            _dbContext.Wallets.Update(wallet);
            await _dbContext.SaveChangesAsync();
        }
    }
}