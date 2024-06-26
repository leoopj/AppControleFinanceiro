using AppControleFinanceiro.Models;
using AppControleFinanceiro.Models.Enums;
using AppControleFinanceiro.Repositories;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
	public TransactionAdd(ITransactionRepository repository)
	{
		InitializeComponent();
        _repository = repository;
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		Navigation.PopModalAsync();
    }

    private void ButtonClicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;

        SaveTransactionInDatabase();

        Navigation.PopModalAsync();

        var count = _repository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Mensagem!", $"Existem {count} registro(s) no banco!", "OK");
    }

    private bool IsValidData()
    { 
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if(string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        }

        double valueResult;
        if(!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out valueResult))
        {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid = false;
        }

        if(!valid)
        {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        }

        return valid;
    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = double.Parse(EntryValue.Text)
        };

        _repository.Add(transaction);
    }
}