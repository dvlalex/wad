using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data.entity;
using data.repository;
using data.sql;

namespace data.service
{
    public interface IHtmlSnippetService
    {
        void CreateHtmlSnippet(HtmlSnippet htmlSnippet);
        void UpdateHtmlSnippet(HtmlSnippet htmlSnippet);
        HtmlSnippet RetrieveHtmlSnippet(int id);

        void DeleteHtmlSnippet(int id);
    }

    public class HtmlSnippetService : IHtmlSnippetService
    {
        private readonly IHtmlSnippetRepository _htmlSnippetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HtmlSnippetService(IHtmlSnippetRepository htmlSnippetRepository, IUnitOfWork unitOfWork)
        {
            if (htmlSnippetRepository == null)
                throw new ArgumentNullException("htmlSnippetRepository");
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            _htmlSnippetRepository = htmlSnippetRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateHtmlSnippet(HtmlSnippet snippet)
        {
            if (snippet == null)
                throw new ArgumentNullException("snippet");

            _htmlSnippetRepository.Add(snippet);
            _unitOfWork.Commit();
        }

        public void UpdateHtmlSnippet(HtmlSnippet snippet)
        {
            if (snippet == null)
                throw new ArgumentNullException("snippet");

            _htmlSnippetRepository.Update(snippet);
            _unitOfWork.Commit();
        }

        public HtmlSnippet RetrieveHtmlSnippet(int id)
        {
            return _htmlSnippetRepository.Get(u => u.Id == id);
        }

        public void DeleteHtmlSnippet(int id)
        {
            _htmlSnippetRepository.Delete(u => u.Id == id);
        }

    }
}
