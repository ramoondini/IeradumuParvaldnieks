using IeradumuParvaldnieks.Domain.Entities;
namespace IeradumuParvaldnieks.Services
{
    public interface IIeradumuService //nosaka kadas darbibas servisiem japrot veikt ar ieradumiem
{
    Task<List<Ieradums>> IeladetIeradumusAsync();//ielade ieradumus no json faila
        Task SaglabatIeradumusAsync(List<Ieradums> ieradumi);//saglaba ieradumus json faila
    }
}