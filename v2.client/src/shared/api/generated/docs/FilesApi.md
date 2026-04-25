# FilesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createFile**](#createfile) | **POST** /Files | |
|[**deleteFileById**](#deletefilebyid) | **DELETE** /Files/{id} | |
|[**getFile**](#getfile) | **GET** /Files | |
|[**getFileById**](#getfilebyid) | **GET** /Files/{id} | |
|[**getFileByIdExport**](#getfilebyidexport) | **GET** /Files/{id}/export | |
|[**updateFileById**](#updatefilebyid) | **PUT** /Files/{id} | |
|[**updateFileByIdCopy**](#updatefilebyidcopy) | **PUT** /Files/{id}/copy | |
|[**updateFileByIdImport**](#updatefilebyidimport) | **PUT** /Files/{id}/import | |
|[**updateFileByIdMove**](#updatefilebyidmove) | **PUT** /Files/{id}/move | |

# **createFile**
> createFile()


### Example

```typescript
import {
    FilesApi,
    Configuration,
    FileDto
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let body: FileDto; // (optional)

const { status, data } = await apiInstance.createFile(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **FileDto**|  | |


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteFileById**
> deleteFileById()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let pernament: boolean; // (optional) (default to undefined)

const { status, data } = await apiInstance.deleteFileById(
    id,
    pernament
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **pernament** | [**boolean**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getFile**
> FileResponseList getFile()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let locaction: string; // (optional) (default to undefined)
let fileType: number; // (optional) (default to undefined)
let subject: string; // (optional) (default to undefined)
let year: number; // (optional) (default to undefined)
let semester: number; // (optional) (default to undefined)
let includeImages: boolean; // (optional) (default to undefined)

const { status, data } = await apiInstance.getFile(
    order,
    page,
    pageSize,
    orderBy,
    title,
    locaction,
    fileType,
    subject,
    year,
    semester,
    includeImages
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **locaction** | [**string**] |  | (optional) defaults to undefined|
| **fileType** | [**number**] |  | (optional) defaults to undefined|
| **subject** | [**string**] |  | (optional) defaults to undefined|
| **year** | [**number**] |  | (optional) defaults to undefined|
| **semester** | [**number**] |  | (optional) defaults to undefined|
| **includeImages** | [**boolean**] |  | (optional) defaults to undefined|


### Return type

**FileResponseList**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getFileById**
> any getFileById()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getFileById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**any**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getFileByIdExport**
> File getFileByIdExport()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let newLocaction: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getFileByIdExport(
    id,
    newLocaction
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **newLocaction** | [**string**] |  | (optional) defaults to undefined|


### Return type

**File**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateFileById**
> updateFileById()


### Example

```typescript
import {
    FilesApi,
    Configuration,
    FileDto
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let body: FileDto; // (optional)

const { status, data } = await apiInstance.updateFileById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **FileDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateFileByIdCopy**
> updateFileByIdCopy()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let newLocaction: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.updateFileByIdCopy(
    id,
    newLocaction
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **newLocaction** | [**string**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateFileByIdImport**
> updateFileByIdImport()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let newLocaction: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.updateFileByIdImport(
    id,
    newLocaction
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **newLocaction** | [**string**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateFileByIdMove**
> updateFileByIdMove()


### Example

```typescript
import {
    FilesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new FilesApi(configuration);

let id: string; // (default to undefined)
let newLocaction: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.updateFileByIdMove(
    id,
    newLocaction
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|
| **newLocaction** | [**string**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

