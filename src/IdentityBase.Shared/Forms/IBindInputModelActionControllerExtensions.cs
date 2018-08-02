// Copyright (c) Russlan Akiev. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace IdentityBase.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using ServiceBase.Extensions;

    public static class IBindInputModelActionControllerExtensions
    {
        public static async Task<CreateViewModelResult>
            CreateFormViewModelAsync<TCreateViewModelAction>(
            this ControllerBase controller,
            object defaultViewModel = null)
            where TCreateViewModelAction : ICreateViewModelAction
        {
            CreateViewModelContext context = new CreateViewModelContext(
                controller);

            IEnumerable<TCreateViewModelAction> actions = controller
               .HttpContext
               .RequestServices
               .GetServices<TCreateViewModelAction>();

            if (defaultViewModel != null)
            {
                context.Items["DefaultViewModel"] = defaultViewModel;
            }

            // TODO: filter by step and sort topologically

            foreach (var formComponent in actions)
            {
                await formComponent.ExecuteAsync(context);

                if (context.Cancel)
                {
                    break;
                }
            }

            // Dont mind the rocket science with reverses and stuff
            IEnumerable<FormElement> formElements = context.FormElements
                .AsEnumerable()
                .Reverse()
                .TopologicalSort(x => context.FormElements.Where(c =>
                     !String.IsNullOrWhiteSpace(c.Name) &&
                     !String.IsNullOrWhiteSpace(x.Before) &&
                     c.Name.Equals(x.Before,
                        StringComparison.InvariantCultureIgnoreCase)))
                .Reverse();

            return new CreateViewModelResult(
                context.Items,
                formElements
            );
        }

        public async static Task<BindInputModelResult>
            BindFormInputModelAsync<TBindInputModelAction>(
            this ControllerBase controller)
            where TBindInputModelAction : IBindInputModelAction
        {
            BindInputModelContext context =
                new BindInputModelContext(controller);

            IEnumerable<TBindInputModelAction> actions = controller
                .HttpContext
                .RequestServices
                .GetServices<TBindInputModelAction>();

            // TODO: filter by step and sort topologically
            foreach (var formComponent in actions)
            {
                await formComponent.ExecuteAsync(context);

                if (context.Cancel)
                {
                    break;
                }
            }

            return new BindInputModelResult(context.Items);
        }

        public static async Task HandleFormInputModelAsync
            <TIHandleInputModelAction>(
            this ControllerBase controller)
            where TIHandleInputModelAction : IHandleInputModelAction
        {
            HandleInputModelContext context = new HandleInputModelContext(
                controller);

            IEnumerable<TIHandleInputModelAction> actions = controller
                .HttpContext
                .RequestServices
                .GetServices<TIHandleInputModelAction>();

            // TODO: filter by step and sort topologically
            foreach (var formComponent in actions)
            {
                await formComponent.ExecuteAsync(context);

                if (context.Cancel)
                {
                    break;
                }
            }
        }
    }
}
