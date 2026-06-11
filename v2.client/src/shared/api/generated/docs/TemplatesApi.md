# TemplatesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Templates | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Templates/{id} | |
|[**get**](#get) | **GET** /api/Templates | |
|[**getById**](#getbyid) | **GET** /api/Templates/{id} | |
|[**getRules**](#getrules) | **GET** /api/Templates/rules | |
|[**updateById**](#updatebyid) | **PUT** /api/Templates/{id} | |
|[**updateByIdGenerate**](#updatebyidgenerate) | **PUT** /api/Templates/{id}/generate | |

# **create**
> create()


### Example

```typescript
import {
    TemplatesApi,
    Configuration,
    Template
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let template: Template; // (optional)

const { status, data } = await apiInstance.create(
    template
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **template** | **Template**|  | |


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
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
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

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

# **get**
> TemplateResponseList get()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let title: string; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let page: number; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    title,
    pageSize,
    page
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|


### Return type

**TemplateResponseList**

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
> Template getById()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

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

**Template**

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

# **getRules**
> RulesDto getRules()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let title: string; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let page: number; // (optional) (default to undefined)

const { status, data } = await apiInstance.getRules(
    title,
    pageSize,
    page
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|


### Return type

**RulesDto**

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
> updateById()


### Example

```typescript
import {
    TemplatesApi,
    Configuration,
    Template
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let id: string; // (default to undefined)
let template: Template; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    template
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **template** | **Template**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdGenerate**
> updateByIdGenerate()


### Example

```typescript
import {
    TemplatesApi,
    Configuration,
    EmailGenerationDto
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let id: string; // (default to undefined)
let emailGenerationDto: EmailGenerationDto; // (optional)

const { status, data } = await apiInstance.updateByIdGenerate(
    id,
    emailGenerationDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **emailGenerationDto** | **EmailGenerationDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

