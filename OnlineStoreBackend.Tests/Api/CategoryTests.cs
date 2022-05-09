using System;
using System.Threading.Tasks;
using OnlineStoreBackend.Api.Models.Category;
using OnlineStoreBackend.ApiClient;
using Xunit;

namespace OnlineStoreBackend.Tests.Api;

[Collection(Constants.CollectionDefinition)]
public class CategoryTests
{
    private readonly ICategoryApiClient _categoryClient;

    public CategoryTests(TestFixture testFixture)
    {
        _categoryClient = testFixture.CategoryApiClient;
    }

    [Fact]
    public async Task Create_Get_GetAll_Delete_Category_Success()
    {
        var req = new AddCategoryRequest
        {
            Name = "Create_Get_GetAll_Delete_Category_Success",
            IsActive = true,
            Path = null,
            Parent = null,
            Description = "Some cat description"
        };
        var create = await _categoryClient.Create(req);
        Assert.NotNull(create.Id);
        await Task.Delay(TimeSpan.FromSeconds(1));

        var get = await _categoryClient.Get(create.Id);
        Assert.NotNull(get.Result.Id);
        Assert.NotNull(get.Result.Path);
        Assert.Equal(req.Name, get.Result.Name);
        Assert.Equal(req.Description, get.Result.Description);
        Assert.Equal(req.IsActive, get.Result.IsActive);
        
        var getAll = await _categoryClient.GetAll();
        Assert.NotEmpty(getAll.Result);
        Assert.Equal(get.Result.Id, getAll.Result[0].Id);
        Assert.Equal(get.Result.Path, getAll.Result[0].Path);
        Assert.Equal(req.Name, getAll.Result[0].Name);
        Assert.Equal(req.Description, getAll.Result[0].Description);
        Assert.Equal(req.IsActive, getAll.Result[0].IsActive);

        await _categoryClient.Delete(create.Id);
    }

    [Fact]
    public async Task CreateCategory_ParentNotExists_Fail()
    {
        var req = new AddCategoryRequest
        {
            Name = "CreateCategory_ParentNotExists_Fail",
            IsActive = true,
            Path = null,
            Parent = "-",
            Description = "Some cat description"
        };
        await Assert.ThrowsAsync<ApiException>(() => _categoryClient.Create(req));
    }

    [Fact]
    public async Task CreateCategory_PathDublicate_Fail()
    {
        var req = new AddCategoryRequest
        {
            Name = "CreateCategory_PathDublicate_Fail",
            IsActive = true,
            Path = "CreateCategory_PathDublicate_Fail",
            Parent = null,
            Description = "Some cat description"
        };
        var samePathReq = new AddCategoryRequest
        {
            Name = "CreateCategory_PathDublicate_Fail 1",
            IsActive = true,
            Path = req.Path,
            Parent = null,
            Description = "Some cat description 1"
        };

        var create = await _categoryClient.Create(req);
        await Task.Delay(TimeSpan.FromSeconds(1));
        await Assert.ThrowsAsync<ApiException>(() => _categoryClient.Create(samePathReq));

        await _categoryClient.Delete(create.Id);
    }

    [Fact]
    public async Task UpdateCategory_Success()
    {
        var req = new AddCategoryRequest
        {
            Name = "UpdateCategory_Success",
            IsActive = true,
            Path = "UpdateCategory_Success",
            Parent = null,
            Description = "Some cat description"
        };

        var create = await _categoryClient.Create(req);
        await Task.Delay(TimeSpan.FromSeconds(1));

        var updateReq = new UpdateCategoryRequest
        {
            Name = req.Name + " updated",
            IsActive = false,
            Path = req.Path + "_updated",
            Parent = null,
            Description = req.Description + " updated"
        };

        await _categoryClient.Update(create.Id, updateReq);
        var get = await _categoryClient.Get(create.Id);
        Assert.Equal(updateReq.Path, get.Result.Path);
        Assert.Equal(updateReq.Name, get.Result.Name);
        Assert.Equal(updateReq.Description, get.Result.Description);
        Assert.Equal(updateReq.IsActive, get.Result.IsActive);

        await _categoryClient.Delete(create.Id);
    }

    [Fact]
    public async Task UpdateCategory_ParentNotExists_Fail()
    {
        var req = new AddCategoryRequest
        {
            Name = "UpdateCategory_ParentNotExists_Fail",
            IsActive = true,
            Path = "UpdateCategory_ParentNotExists_Fail",
            Parent = null,
            Description = "Some cat description"
        };
        
        var create = await _categoryClient.Create(req);
        await Task.Delay(TimeSpan.FromSeconds(1));

        var updateReq = new UpdateCategoryRequest
        {
            Name = req.Name,
            IsActive = req.IsActive,
            Path = req.Path,
            Description = req.Description,
            Parent = "-"
        };

        await Assert.ThrowsAsync<ApiException>(() => _categoryClient.Update(create.Id, updateReq));
        await _categoryClient.Delete(create.Id);
    }
    
    [Fact]
    public async Task DeleteCategory_IdNotExists_Fail()
    {
        var id = "-";
        await Assert.ThrowsAsync<ApiException>(() => _categoryClient.Delete(id));
    }
    
    [Fact]
    public async Task UpdateCategory_IdNotExists_Fail()
    {
        var id = "-";
        var req = new UpdateCategoryRequest
        {
            Name = "UpdateCategory_IdNotExists_Fail",
            IsActive = true,
            Path = "UpdateCategory_IdNotExists_Fail",
            Parent = null,
            Description = "Some cat description"
        };
        await Assert.ThrowsAsync<ApiException>(() => _categoryClient.Update(id, req));
    }
    
    
}