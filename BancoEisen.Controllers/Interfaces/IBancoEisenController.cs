namespace BancoEisen.Controllers.Interfaces
{
    public interface IBancoEisenController<T>
    {
        T Consultar(int id);
        T[] Todos();
    }
}
