using AppControleFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using System.Runtime.CompilerServices;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    private ITransactionRepository _repository;
    public TransactionList(ITransactionRepository repository)
    {
        this._repository = repository;
        
        InitializeComponent();

        Reload();

        //Recebe uma mensagem do tipo string para atualizar a tela/grid, a mensagem também pode ser do tipo objeto
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        //Atribui lista de dados para coleção ser apresentada no front
        CollectionViewTransaction.ItemsSource = _repository.GetAll();
    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
	{
        //Cria uma nova estância a cada solicitação da tela de adicionar
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
		Navigation.PushModalAsync(transactionAdd);
	}

    private void OnButtonClicked_To_TransactionEdit(object sender, EventArgs e)
    {
        //Cria uma nova estância a cada solicitação da tela de editar
        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        Navigation.PushModalAsync(transactionEdit);
    }
}