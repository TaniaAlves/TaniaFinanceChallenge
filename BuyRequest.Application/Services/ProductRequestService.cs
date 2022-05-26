using AutoMapper;
using BuyRequest.Application.Interfaces;
using BuyRequest.Data.Repositorys.ProductRequest;

namespace BuyRequest.Application.Services
{
    public class ProductRequestService : IProductRequestService
    {
        private readonly IProductRequestRepository _productRequestRepository;
        private readonly IMapper _mapper;

        BuyRequest.Domain.Entities.ProductRequest productRequest = new();

        public ProductRequestService(IProductRequestRepository productRequestRepository, IMapper mapper)
        {
            _productRequestRepository = productRequestRepository;
            _mapper = mapper;
        }

        //public async Task<decimal> PostProduct(List<ProductRequestDTO> products, Guid requestId)
        //{
        //    var lastProductCategory = products.FirstOrDefault().ProductCategory;
        //    decimal price = 0;

        //    foreach (var item in products)
        //    {
        //        if (item.ProductCategory != lastProductCategory)
        //        {
        //            var errors = _productRequestRepository.BadRequestMessage(productRequest, "Products must have the same category.");
        //            var error = ValidatorErrors(errors);
        //            throw new Exception(error);
        //        }

        //        var mapProduct = _mapper.Map(item, productRequest);

        //        var productValidator = new ProductRequestValidator();
        //        var prodValid = productValidator.Validate(mapProduct);

        //        if (prodValid.IsValid)
        //        {
        //            mapProduct.RequestId = requestId;
        //            mapProduct.Id = Guid.NewGuid();
        //            mapProduct.ProductId = Guid.NewGuid();

        //            await _productRequestRepository.CreateAsync(mapProduct);

        //            price += productRequest.Total;
        //        }
        //        else
        //        {
        //            var errors = new ErrorMessage<Domain.Entities.ProductRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
        //                prodValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), productRequest);

        //            var error = ValidatorErrors(errors);
        //            throw new Exception(error);
        //        }
        //    }

        //    return price;
        //}

        //public string ValidatorErrors(ErrorMessage<Domain.Entities.ProductRequest> errorList)
        //{
        //    string error = "";
        //    foreach (var item in errorList.Message)
        //        error += item + " ";

        //    return error;
        //}

        //public async Task<decimal> UpdateByIdAsync(Guid id, BuyRequestDTO orderInput) //status?
        //{

        //    //var findRequest = await _buyRequestRepository.GetByIdAsync(id);
        //    var findProducts = _productRequestRepository.GetAllByRequestId(id).ToList();

        //    #region Validations
        //    if (findProducts == null)
        //    {
        //        var errors = _productRequestRepository.NotFoundMessage(productRequest);
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }

        //    if (orderInput.Products.FirstOrDefault().ProductCategory == ProductCategory.Physical && orderInput.Status == Status.WaitingDownload)
        //    {
        //        var errors = _productRequestRepository.BadRequestMessage(productRequest, "Physical products can't be set to 'Waiting To Download' Status");
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }
        //    else if (orderInput.Products.FirstOrDefault().ProductCategory == ProductCategory.Digital && orderInput.Status == Status.WaitingDelivery)
        //    {
        //        var errors = _productRequestRepository.BadRequestMessage(productRequest, "Digital products can't be set to 'Waiting To Delivery' Status");
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }
        //    #endregion

        //    //var oldStatus = findRequest.Status;
        //    //var totalValueOld = findRequest.TotalValue;
        //    int small = findProducts.Count();
        //    decimal findRequestPrice = 0;
        //    //findRequest.Price = 0;

        //    if (findProducts.Count() < orderInput.Products.Count())
        //    {
        //        for (int i = findProducts.Count(); i < orderInput.Products.Count(); i++) //se novos > existentes -> criar novos
        //        {
        //            var mapProduct = _mapper.Map(orderInput.Products[i], productRequest);

        //            #region buyRequestUpdate
        //            mapProduct.RequestId = id;
        //            mapProduct.Id = Guid.NewGuid();
        //            mapProduct.ProductId = Guid.NewGuid();
        //            mapProduct.Total = mapProduct.Pvp * mapProduct.Quantity;
        //            findRequestPrice += mapProduct.Total;
        //            #endregion

        //            var prodValidator = new ProductRequestValidator();
        //            var prodValid = prodValidator.Validate(mapProduct);

        //            if (prodValid.IsValid)
        //                await _productRequestRepository.CreateAsync(mapProduct);
        //            else
        //            {
        //                var errors = new ErrorMessage<Domain.Entities.ProductRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(),
        //                prodValid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), productRequest);

        //                var error = ValidatorErrors(errors);
        //                throw new Exception(error);
        //            }
        //        }
        //    }
        //    else if (findProducts.Count() > orderInput.Products.Count()) //se novos produtos < qtdd existente _> apagar antigos
        //    {
        //        small = orderInput.Products.Count();

        //        for (int i = orderInput.Products.Count(); i < findProducts.Count(); i++)
        //            await _productRequestRepository.DeleteAsync(findProducts[i]);
        //    }

        //    for (int i = 0; i < small; i++)      //enquanto sao iguais
        //    {
        //        findProducts[i].Total = orderInput.Products[i].Pvp * orderInput.Products[i].Quantity;
        //        var mapProduct = _mapper.Map(orderInput.Products[i], findProducts[i]);
        //        mapProduct.ProductId = Guid.NewGuid();

        //        await _productRequestRepository.UpdateAsync(mapProduct);

        //        findRequestPrice += findProducts[i].Total;
        //    }

        //    //findRequest.TotalValue = findRequest.Price - (findRequest.Price * (findRequest.DiscountValue / 100));

        //    //var map = _mapper.Map(orderInput, findRequest);

        //    //var validator = new BuyRequestValidator();
        //    //var valid = validator.Validate(map);

        //    //if (valid.IsValid)
        //    //    await _buyRequestRepository.UpdateAsync(map);
        //    //else
        //    //{
        //    //    var news = new ErrorMessage<BuyRequest>(HttpStatusCode.BadRequest.GetHashCode().ToString(), valid.Errors.ConvertAll(x => x.ErrorMessage.ToString()), buyRequest);
        //    //    return StatusCode((int)HttpStatusCode.BadRequest, news);
        //    //}

        //    //if (findRequest.Status == Status.Finalized)
        //    //{
        //    //    var type = FinanceChallengeTania.Domain.Entities.Type.Receive;
        //    //    var recentValue = map.TotalValue;
        //    //    string description = $"Financial transaction order id: {findRequest.Id}";

        //    //    if (map.Status == oldStatus && map.Status == Status.Finalized && totalValueOld > map.TotalValue)
        //    //    {
        //    //        description = $"Diference purchase order id: {findRequest.Id}";
        //    //        recentValue = map.TotalValue - totalValueOld;
        //    //        type = FinanceChallengeTania.Domain.Entities.Type.Payment;
        //    //    }

        //    //    var response = await _bankRecordRepository.CreateBankRecord(Origin.PurchaseRequest, map.Id, description,
        //    //      type, recentValue);

        //    //    if (!response.IsSuccessStatusCode)
        //    //    {
        //    //        var result = _bankRecordRepository.BadRequestMessage(bank, response.Content.ToString());
        //    //        return StatusCode((int)HttpStatusCode.BadRequest, result);
        //    //    }
        //    //}
        //    return findRequestPrice;

        //}

        //public async Task UpdateState(Guid id, Status status)
        //{
        //    var requestProductsToUpdate = _productRequestRepository.GetAllByRequestId(id).FirstOrDefault();

        //    #region Validations
        //    if (requestProductsToUpdate == null)
        //    {
        //        var errors = _productRequestRepository.NotFoundMessage(productRequest);
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }

        //    if (requestProductsToUpdate.ProductCategory == ProductCategory.Physical && status == Status.WaitingDownload)
        //    {
        //        var errors = _productRequestRepository.BadRequestMessage(productRequest, "Physical products can't be set to 'Waiting To Download' Status");
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }
        //    else if (requestProductsToUpdate.ProductCategory == ProductCategory.Digital && status == Status.WaitingDelivery)
        //    {
        //        var errors = _productRequestRepository.BadRequestMessage(productRequest, "Digital products can't be set to 'Waiting To Delivery' Status");
        //        var error = ValidatorErrors(errors);
        //        throw new Exception(error);
        //    }
        //    #endregion

        //}
    }
}
