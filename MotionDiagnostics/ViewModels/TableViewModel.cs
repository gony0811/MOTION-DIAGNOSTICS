using EPLE.Data.Entity;
using System;
using System.Windows;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;

[AddINotifyPropertyChangedInterface]
public partial class TableViewModel
{
    public DeviceConfigEntity DeviceConfig { get; set; }

    private readonly DeviceSaveDelegate _updateFunction;
    private readonly DeviceDeleteDelegate _deleteFunction;

    public delegate void DeviceSaveDelegate(DeviceConfigEntity entity);
    public delegate void DeviceDeleteDelegate(TableViewModel viewModel);



    public TableViewModel(DeviceSaveDelegate updateFunction, DeviceDeleteDelegate deleteDelegate)
        : this(new DeviceConfigEntity(), updateFunction, deleteDelegate) { }

    public TableViewModel(DeviceConfigEntity deviceConfig, DeviceSaveDelegate updateFunction, DeviceDeleteDelegate deleteFunction)
    {
        DeviceConfig = deviceConfig ?? throw new ArgumentNullException(nameof(deviceConfig));
        _updateFunction = updateFunction ?? throw new ArgumentNullException(nameof(updateFunction));
        _deleteFunction = deleteFunction ?? throw new ArgumentNullException(nameof(deleteFunction));
    }

    [DelegateCommand]
    public void Save()
    {
        var validationResult = DeviceConfig.Validate();

        if(validationResult.IsValid)
        {
            _updateFunction(DeviceConfig);
            ShowMessage("저장되었습니다.");
            return;
        }
        
        ShowError($"잘못된 입력값입니다. \n 이유: {validationResult.Errors[0]}");
        
    }

    [DelegateCommand]
    public void Delete()
    {
        _deleteFunction(this);
    }

    private void ShowMessage(string message)
    {
        // 메시지 표시 로직 (UI 로직 분리 시 변경 가능)
        MessageBox.Show(message, "알림", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ShowError(string message)
    {
        // 오류 표시 로직 (UI 로직 분리 시 변경 가능)
        MessageBox.Show(message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
    }

}
