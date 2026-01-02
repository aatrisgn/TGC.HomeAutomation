namespace TGC.HomeAutomation.Application.Abstractions;

public interface IQueryHandler
{
	public Task<IQueryResponse> Handle<TQuery>(TQuery command) where TQuery : IQuery;
	public bool Accepts(IQuery query);
}