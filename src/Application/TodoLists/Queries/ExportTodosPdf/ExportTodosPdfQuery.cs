using System.Diagnostics;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CleanArchitecture.Application.TodoLists.Queries.ExportTodosPdf;

public record ExportTodosPdfQuery : IRequest<Unit>
{
    public int ListId { get; init; }
}

public class ExportTodosPdfQueryHandler : IRequestHandler<ExportTodosPdfQuery, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICsvFileBuilder _fileBuilder;

    public ExportTodosPdfQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    public Task<Unit> Handle(ExportTodosPdfQuery request, CancellationToken cancellationToken)
    {
        var path = "hello.pdf";
        var report = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text("Hello PDF!")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text(Placeholders.LoremIpsum());
                            x.Item().Image(Placeholders.Image(200, 100));
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });
            

        report.GeneratePdf(path);
        Process.Start("explorer.exe", path);

        return Task.FromResult(Unit.Value);
    }
}
