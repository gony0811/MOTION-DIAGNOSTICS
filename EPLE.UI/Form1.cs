using Microsoft.Extensions.Logging;
using EPLE.Manager;
using Microsoft.Extensions.DependencyInjection;
using EPLE.ViewModel;

namespace EPLE.UI
{

    public partial class Form1 : Form
    {
        private readonly ILogger<Form1> logger;
        private readonly IServiceProvider serviceProvider;
        public Form1(ILogger<Form1> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            InitializeComponent();

            logger.LogInformation("Application starting up...");

            var dataManager = serviceProvider.GetRequiredService<DataManager>();

            var value = dataManager.GET_DATA("TEST", out var result);

            dataManager.SET_DATA("TEST", 111, true);


        }
    }
}
