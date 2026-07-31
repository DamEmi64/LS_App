# FilesV2Api

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/FilesV2 | |
|[**createByIdUsers**](#createbyidusers) | **POST** /api/FilesV2/{id}/users | |
|[**deleteById**](#deletebyid) | **DELETE** /api/FilesV2/{id} | |
|[**deleteByIdUsersByUserId**](#deletebyidusersbyuserid) | **DELETE** /api/FilesV2/{id}/users/{userId} | |
|[**get**](#get) | **GET** /api/FilesV2 | |
|[**getById**](#getbyid) | **GET** /api/FilesV2/{id} | |
|[**getByIdDownload**](#getbyiddownload) | **GET** /api/FilesV2/{id}/download | |
|[**getByIdUsers**](#getbyidusers) | **GET** /api/FilesV2/{id}/users | |
|[**updateById**](#updatebyid) | **PUT** /api/FilesV2/{id} | |

# **create**
> FileV2Dto create()


### Example

```typescript
import {
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

let file: File; // (default to undefined)
let title: string; // (default to undefined)
let description: string; // (optional) (default to undefined)
let directoryId: string; // (optional) (default to undefined)
let _public: boolean; // (optional) (default to undefined)

const { status, data } = await apiInstance.create(
    file,
    title,
    description,
    directoryId,
    _public
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **file** | [**File**] |  | defaults to undefined|
| **title** | [**string**] |  | defaults to undefined|
| **description** | [**string**] |  | (optional) defaults to undefined|
| **directoryId** | [**string**] |  | (optional) defaults to undefined|
| **_public** | [**boolean**] |  | (optional) defaults to undefined|


### Return type

**FileV2Dto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: multipart/form-data
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
    FilesV2Api,
    Configuration,
    GrantAccessDto
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

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
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

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
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

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
> Array<FileV2Dto> get()


### Example

```typescript
import {
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

let directoryId: string; // (optional) (default to undefined)
let search: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    directoryId,
    search
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **directoryId** | [**string**] |  | (optional) defaults to undefined|
| **search** | [**string**] |  | (optional) defaults to undefined|


### Return type

**Array<FileV2Dto>**

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
> FileV2Dto getById()


### Example

```typescript
import {
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

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

**FileV2Dto**

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

# **getByIdDownload**
> Media getByIdDownload()


### Example

```typescript
import {
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdDownload(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Media**

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
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

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
> FileV2Dto updateById()


### Example

```typescript
import {
    FilesV2Api,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesV2Api(configuration);

let id: string; // (default to undefined)
let file: File; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let description: string; // (optional) (default to undefined)
let directoryId: string; // (optional) (default to undefined)
let _public: boolean; // (optional) (default to undefined)

const { status, data } = await apiInstance.updateById(
    id,
    file,
    title,
    description,
    directoryId,
    _public
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **file** | [**File**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **description** | [**string**] |  | (optional) defaults to undefined|
| **directoryId** | [**string**] |  | (optional) defaults to undefined|
| **_public** | [**boolean**] |  | (optional) defaults to undefined|


### Return type

**FileV2Dto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

