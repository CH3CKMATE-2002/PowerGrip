namespace Andreas.PowerGrip.Server.Conventions;

public class RemoveControllerConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Check if the controller has the [ExcludeFromProduction] attribute
        if (controller.ControllerType.GetCustomAttribute<ExcludeFromProductionAttribute>() != null)
        {
            controller.ApiExplorer.IsVisible = false; // Hide it from API docs
            controller.Actions.Clear(); // Remove all endpoints
        }
    }
}
