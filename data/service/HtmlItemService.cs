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
    public interface IHtmlItemService
    {
        void CreateHtmlItem(HtmlItem htmlItem);
        void UpdateHtmlItem(HtmlItem htmlItem);
        HtmlItem RetrieveHtmlItem(int id);
        List<HtmlItem> RetrieveHtmlItemsForUser(int userid);
    }

    public class HtmlItemService : IHtmlItemService
    {
        private readonly IHtmlItemRepository _htmlItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HtmlItemService(IHtmlItemRepository htmlItemRepository, IUnitOfWork unitOfWork)
        {
            if (htmlItemRepository == null)
                throw new ArgumentNullException("htmlItemRepository");
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            _htmlItemRepository = htmlItemRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateHtmlItem(HtmlItem Item)
        {
            if (Item == null)
                throw new ArgumentNullException("Item");

            _htmlItemRepository.Add(Item);
            _unitOfWork.Commit();
        }

        public void UpdateHtmlItem(HtmlItem Item)
        {
            if (Item == null)
                throw new ArgumentNullException("Item");

            _htmlItemRepository.Update(Item);
            _unitOfWork.Commit();
        }

        public HtmlItem RetrieveHtmlItem(int id)
        {
            return _htmlItemRepository.Get(u => u.Id == id);
        }

        public List<HtmlItem> RetrieveHtmlItemsForUser(int userid)
        {
            return _htmlItemRepository.GetAll().Where(h => h.User.UserId == userid).ToList();
        }
    }
}
