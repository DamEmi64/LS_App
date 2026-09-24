# DirectoriesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Directories | |
|[**createByIdUsers**](#createbyidusers) | **POST** /api/Directories/{id}/users | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Directories/{id} | |
|[**deleteByIdUsersByUserId**](#deletebyidusersbyuserid) | **DELETE** /api/Directories/{id}/users/{userId} | |
|[**get**](#get) | **GET** /api/Directories | |
|[**getById**](#getbyid) | **GET** /api/Directories/{id} | |
|[**getByIdUsers**](#getbyidusers) | **GET** /api/Directories/{id}/users | |
|[**updateById**](#updatebyid) | **PUT** /api/Directories/{id} | |

# **create**
> DirectoryDto create()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration,
    CreateDirectoryDto
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let createDirectoryDto: CreateDirectoryDto; // (optional)

const { status, data } = await apiInstance.create(
    createDirectoryDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createDirectoryDto** | **CreateDirectoryDto**|  | |


### Return type

**DirectoryDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **createByIdUsers**
> FileUserDto createByIdUsers()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration,
    GrantAccessDto
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)
let grantAccessDto: GrantAccessDto; // (optional)

const { status, data } = await apiInstance.createByIdUsers(
    id,
    grantAccessDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **grantAccessDto** | **GrantAccessDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**FileUserDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteById**
> deleteById()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteByIdUsersByUserId**
> deleteByIdUsersByUserId()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)
let userId: string; // (default to undefined)

const { status, data } = await apiInstance.deleteByIdUsersByUserId(
    id,
    userId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **userId** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get**
> Array<DirectoryDto> get()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let parentId: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    parentId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **parentId** | [**string**] |  | (optional) defaults to undefined|


### Return type

**Array<DirectoryDto>**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getById**
> DirectoryDto getById()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**DirectoryDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getByIdUsers**
> Array<FileUserDto> getByIdUsers()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdUsers(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Array<FileUserDto>**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateById**
> DirectoryDto updateById()


### Example

```typescript
import {
    DirectoriesApi,
    Configuration,
    UpdateDirectoryDto
} from './api';

const configuration = new Configuration();
const apiInstance = new DirectoriesApi(configuration);

let id: string; // (default to undefined)
let updateDirectoryDto: UpdateDirectoryDto; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    updateDirectoryDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **updateDirectoryDto** | **UpdateDirectoryDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

**DirectoryDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

