# StoriesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Stories | |
|[**createImport**](#createimport) | **POST** /api/Stories/import | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Stories/{id} | |
|[**get**](#get) | **GET** /api/Stories | |
|[**getById**](#getbyid) | **GET** /api/Stories/{id} | |
|[**getByIdDraft**](#getbyiddraft) | **GET** /api/Stories/{id}/draft | |
|[**getByIdExport**](#getbyidexport) | **GET** /api/Stories/{id}/export | |
|[**getByIdSummary**](#getbyidsummary) | **GET** /api/Stories/{id}/summary | |
|[**getDraft**](#getdraft) | **GET** /api/Stories/draft | |
|[**updateById**](#updatebyid) | **PUT** /api/Stories/{id} | |
|[**updateByIdEnd**](#updatebyidend) | **PUT** /api/Stories/{id}/end | |
|[**updateByIdFirebase**](#updatebyidfirebase) | **PUT** /api/Stories/{id}/firebase | |
|[**updateByIdStart**](#updatebyidstart) | **PUT** /api/Stories/{id}/start | |
|[**updateByIdSummary**](#updatebyidsummary) | **PUT** /api/Stories/{id}/summary | |

# **create**
> create()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let title: string; // (default to undefined)
let description: string; // (default to undefined)
let files: Array<File>; // (optional) (default to undefined)

const { status, data } = await apiInstance.create(
    title,
    description,
    files
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **title** | [**string**] |  | defaults to undefined|
| **description** | [**string**] |  | defaults to undefined|
| **files** | **Array&lt;File&gt;** |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **createImport**
> createImport()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let file: string; // (default to undefined)
let converterType: number; // (optional) (default to undefined)
let externalUrl: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.createImport(
    file,
    converterType,
    externalUrl
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **file** | [**string**] |  | defaults to undefined|
| **converterType** | [**number**] |  | (optional) defaults to undefined|
| **externalUrl** | [**string**] |  | (optional) defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: multipart/form-data
 - **Accept**: Not defined


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
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

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

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get**
> StoryResponseList get()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let startFrom: string; // (optional) (default to undefined)
let startTo: string; // (optional) (default to undefined)
let endFrom: string; // (optional) (default to undefined)
let endTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    order,
    page,
    pageSize,
    orderBy,
    title,
    startFrom,
    startTo,
    endFrom,
    endTo
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
| **startFrom** | [**string**] |  | (optional) defaults to undefined|
| **startTo** | [**string**] |  | (optional) defaults to undefined|
| **endFrom** | [**string**] |  | (optional) defaults to undefined|
| **endTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**StoryResponseList**

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

# **getById**
> Story getById()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

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

**Story**

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

# **getByIdDraft**
> Story getByIdDraft()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdDraft(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Story**

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

# **getByIdExport**
> File getByIdExport()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdExport(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


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

# **getByIdSummary**
> File getByIdSummary()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdSummary(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


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

# **getDraft**
> StoryResponseList getDraft()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let startFrom: string; // (optional) (default to undefined)
let startTo: string; // (optional) (default to undefined)
let endFrom: string; // (optional) (default to undefined)
let endTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getDraft(
    order,
    page,
    pageSize,
    orderBy,
    title,
    startFrom,
    startTo,
    endFrom,
    endTo
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
| **startFrom** | [**string**] |  | (optional) defaults to undefined|
| **startTo** | [**string**] |  | (optional) defaults to undefined|
| **endFrom** | [**string**] |  | (optional) defaults to undefined|
| **endTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**StoryResponseList**

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

# **updateById**
> updateById()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    StoryDto
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: StoryDto; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **StoryDto**|  | |
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

# **updateByIdEnd**
> updateByIdEnd()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdEnd(
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

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdFirebase**
> updateByIdFirebase()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    SummaryModel
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: SummaryModel; // (optional)

const { status, data } = await apiInstance.updateByIdFirebase(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **SummaryModel**|  | |
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

# **updateByIdStart**
> updateByIdStart()


### Example

```typescript
import {
    StoriesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdStart(
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

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdSummary**
> updateByIdSummary()


### Example

```typescript
import {
    StoriesApi,
    Configuration,
    SummaryModel
} from './api';

const configuration = new Configuration();
const apiInstance = new StoriesApi(configuration);

let id: string; // (default to undefined)
let body: SummaryModel; // (optional)

const { status, data } = await apiInstance.updateByIdSummary(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **SummaryModel**|  | |
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

