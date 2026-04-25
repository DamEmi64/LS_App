# TemplatesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createTemplate**](#createtemplate) | **POST** /Templates | |
|[**deleteTemplateById**](#deletetemplatebyid) | **DELETE** /Templates/{id} | |
|[**getTemplate**](#gettemplate) | **GET** /Templates | |
|[**getTemplateById**](#gettemplatebyid) | **GET** /Templates/{id} | |
|[**getTemplateRule**](#gettemplaterule) | **GET** /Templates/rules | |
|[**updateTemplateById**](#updatetemplatebyid) | **PUT** /Templates/{id} | |
|[**updateTemplateByIdGenerate**](#updatetemplatebyidgenerate) | **PUT** /Templates/{id}/generate | |

# **createTemplate**
> createTemplate()


### Example

```typescript
import {
    TemplatesApi,
    Configuration,
    Template
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let body: Template; // (optional)

const { status, data } = await apiInstance.createTemplate(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **Template**|  | |


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

# **deleteTemplateById**
> deleteTemplateById()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteTemplateById(
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

# **getTemplate**
> TemplateResponseList getTemplate()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let title: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getTemplate(
    title
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **title** | [**string**] |  | (optional) defaults to undefined|


### Return type

**TemplateResponseList**

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

# **getTemplateById**
> Template getTemplateById()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getTemplateById(
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

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getTemplateRule**
> RulesDto getTemplateRule()


### Example

```typescript
import {
    TemplatesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new TemplatesApi(configuration);

let title: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getTemplateRule(
    title
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **title** | [**string**] |  | (optional) defaults to undefined|


### Return type

**RulesDto**

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

# **updateTemplateById**
> updateTemplateById()


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
let body: Template; // (optional)

const { status, data } = await apiInstance.updateTemplateById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **Template**|  | |
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

# **updateTemplateByIdGenerate**
> updateTemplateByIdGenerate()


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
let body: EmailGenerationDto; // (optional)

const { status, data } = await apiInstance.updateTemplateByIdGenerate(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **EmailGenerationDto**|  | |
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

