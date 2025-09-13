using EQX.Core.InOut;
using EQX.InOut.Virtual;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines
{
    public class VirtualIO
    {
        private readonly IDInputDevice _inWorkConveyorInput;
        private readonly IDOutputDevice _inWorkConveyorOutput;
        private readonly IDInputDevice _outWorkConveyorInput;
        private readonly IDOutputDevice _outWorkConveyorOutput;
        private readonly IDInputDevice _robotLoadInput;
        private readonly IDOutputDevice _robotLoadOutput;
        private readonly IDInputDevice _vinylCleanInput;
        private readonly IDOutputDevice _vinylCleanOutput;
        private readonly IDInputDevice _fixtureAlignInput;
        private readonly IDOutputDevice _fixtureAlignOutput;
        private readonly IDInputDevice _removeFilmInput;
        private readonly IDOutputDevice _removeFilmOutput;
        private readonly IDInputDevice _transferFixtureInput;
        private readonly IDOutputDevice _transferFixtureOutput;
        private readonly IDInputDevice _detachInput;
        private readonly IDOutputDevice _detachOutput;
        private readonly IDInputDevice _glassTransferInput;
        private readonly IDOutputDevice _glassTransferOutput;
        private readonly IDInputDevice _glassAlignLeftInput;
        private readonly IDOutputDevice _glassAlignLeftOutput;
        private readonly IDInputDevice _glassAlignRightInput;
        private readonly IDOutputDevice _glassAlignRightOutput;
        private readonly IDInputDevice _transferInShuttleLeftInput;
        private readonly IDOutputDevice _transferInShuttleLeftOutput;
        private readonly IDInputDevice _transferInShuttleRightInput;
        private readonly IDOutputDevice _transferInShuttleRightOutput;
        private readonly IDInputDevice _wetCleanLeftInput;
        private readonly IDOutputDevice _wetCleanLeftOutput;
        private readonly IDInputDevice _wetCleanRightInput;
        private readonly IDOutputDevice _wetCleanRightOutput;
        private readonly IDInputDevice _transferRotationLeftInput;
        private readonly IDOutputDevice _transferRotationLeftOutput;
        private readonly IDInputDevice _transferRotationRightInput;
        private readonly IDOutputDevice _transferRotationRightOutput;
        private readonly IDInputDevice _afCleanLeftInput;
        private readonly IDOutputDevice _afCleanLeftOutput;
        private readonly IDInputDevice _afCleanRightInput;
        private readonly IDOutputDevice _afCleanRightOutput;
        private readonly IDInputDevice _unloadTransferLeftInput;
        private readonly IDOutputDevice _unloadTransferLeftOutput;
        private readonly IDInputDevice _unloadTransferRightInput;
        private readonly IDOutputDevice _unloadTransferRightOutput;
        private readonly IDInputDevice _unloadAlignInput;
        private readonly IDOutputDevice _unloadAlignOutput;

        public VirtualIO([FromKeyedServices("InWorkConveyorInput")] IDInputDevice inWorkConveyorInput,
                         [FromKeyedServices("InWorkConveyorOutput")] IDOutputDevice inWorkConveyorOutput,
                         [FromKeyedServices("OutWorkConveyorInput")] IDInputDevice outWorkConveyorInput,
                         [FromKeyedServices("OutWorkConveyorOutput")] IDOutputDevice outWorkConveyorOutput,
                         [FromKeyedServices("RobotLoadInput")] IDInputDevice robotLoadInput,
                         [FromKeyedServices("RobotLoadOutput")] IDOutputDevice robotLoadOutput,
                         [FromKeyedServices("VinylCleanInput")] IDInputDevice vinylCleanInput,
                         [FromKeyedServices("VinylCleanOutput")] IDOutputDevice vinylCleanOutput,
                         [FromKeyedServices("FixtureAlignInput")] IDInputDevice fixtureAlignInput,
                         [FromKeyedServices("FixtureAlignOutput")] IDOutputDevice fixtureAlignOutput,
                         [FromKeyedServices("RemoveFilmInput")] IDInputDevice removeFilmInput,
                         [FromKeyedServices("RemoveFilmOutput")] IDOutputDevice removeFilmOutput,
                         [FromKeyedServices("TransferFixtureInput")] IDInputDevice transferFixtureInput,
                         [FromKeyedServices("TransferFixtureOutput")] IDOutputDevice transferFixtureOutput,
                         [FromKeyedServices("DetachInput")] IDInputDevice detachInput,
                         [FromKeyedServices("DetachOutput")] IDOutputDevice detachOutput,
                         [FromKeyedServices("GlassTransferInput")] IDInputDevice glassTransferInput,
                         [FromKeyedServices("GlassTransferOutput")] IDOutputDevice glassTransferOutput,
                         [FromKeyedServices("GlassAlignLeftInput")] IDInputDevice glassAlignLeftInput,
                         [FromKeyedServices("GlassAlignLeftOutput")] IDOutputDevice glassAlignLeftOutput,
                         [FromKeyedServices("GlassAlignRightInput")] IDInputDevice glassAlignRightInput,
                         [FromKeyedServices("GlassAlignRightOutput")] IDOutputDevice glassAlignRightOutput,
                         [FromKeyedServices("TransferInShuttleLeftInput")] IDInputDevice transferInShuttleLeftInput,
                         [FromKeyedServices("TransferInShuttleLeftOutput")] IDOutputDevice transferInShuttleLeftOutput,
                         [FromKeyedServices("TransferInShuttleRightInput")] IDInputDevice transferInShuttleRightInput,
                         [FromKeyedServices("TransferInShuttleRightOutput")] IDOutputDevice transferInShuttleRightOutput,
                         [FromKeyedServices("WETCleanLeftInput")] IDInputDevice wetCleanLeftInput,
                         [FromKeyedServices("WETCleanLeftOutput")] IDOutputDevice wetCleanLeftOutput,
                         [FromKeyedServices("WETCleanRightInput")] IDInputDevice wetCleanRightInput,
                         [FromKeyedServices("WETCleanRightOutput")] IDOutputDevice wetCleanRightOutput,
                         [FromKeyedServices("TransferRotationLeftInput")] IDInputDevice transferRotationLeftInput,
                         [FromKeyedServices("TransferRotationLeftOutput")] IDOutputDevice transferRotationLeftOutput,
                         [FromKeyedServices("TransferRotationRightInput")] IDInputDevice transferRotationRightInput,
                         [FromKeyedServices("TransferRotationRightOutput")] IDOutputDevice transferRotationRightOutput,
                         [FromKeyedServices("AFCleanLeftInput")] IDInputDevice afCleanLeftInput,
                         [FromKeyedServices("AFCleanLeftOutput")] IDOutputDevice afCleanLeftOutput,
                         [FromKeyedServices("AFCleanRightInput")] IDInputDevice afCleanRightInput,
                         [FromKeyedServices("AFCleanRightOutput")] IDOutputDevice afCleanRightOutput,
                         [FromKeyedServices("UnloadTransferLeftInput")] IDInputDevice unloadTransferLeftInput,
                         [FromKeyedServices("UnloadTransferLeftOutput")] IDOutputDevice unloadTransferLeftOutput,
                         [FromKeyedServices("UnloadTransferRightInput")] IDInputDevice unloadTransferRightInput,
                         [FromKeyedServices("UnloadTransferRightOutput")] IDOutputDevice unloadTransferRightOutput,
                         [FromKeyedServices("UnloadAlignInput")] IDInputDevice unloadAlignInput,
                         [FromKeyedServices("UnloadAlignOutput")] IDOutputDevice unloadAlignOutput)
        {
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _robotLoadInput = robotLoadInput;
            _robotLoadOutput = robotLoadOutput;
            _vinylCleanInput = vinylCleanInput;
            _vinylCleanOutput = vinylCleanOutput;
            _fixtureAlignInput = fixtureAlignInput;
            _fixtureAlignOutput = fixtureAlignOutput;
            _removeFilmInput = removeFilmInput;
            _removeFilmOutput = removeFilmOutput;
            _transferFixtureInput = transferFixtureInput;
            _transferFixtureOutput = transferFixtureOutput;
            _detachInput = detachInput;
            _detachOutput = detachOutput;
            _glassTransferInput = glassTransferInput;
            _glassTransferOutput = glassTransferOutput;
            _glassAlignLeftInput = glassAlignLeftInput;
            _glassAlignLeftOutput = glassAlignLeftOutput;
            _glassAlignRightInput = glassAlignRightInput;
            _glassAlignRightOutput = glassAlignRightOutput;
            _transferInShuttleLeftInput = transferInShuttleLeftInput;
            _transferInShuttleLeftOutput = transferInShuttleLeftOutput;
            _transferInShuttleRightInput = transferInShuttleRightInput;
            _transferInShuttleRightOutput = transferInShuttleRightOutput;
            _wetCleanLeftInput = wetCleanLeftInput;
            _wetCleanLeftOutput = wetCleanLeftOutput;
            _wetCleanRightInput = wetCleanRightInput;
            _wetCleanRightOutput = wetCleanRightOutput;

            _transferRotationLeftInput = transferRotationLeftInput;
            _transferRotationLeftOutput = transferRotationLeftOutput;
            _transferRotationRightInput = transferRotationRightInput;
            _transferRotationRightOutput = transferRotationRightOutput;

            _afCleanLeftInput = afCleanLeftInput;
            _afCleanLeftOutput = afCleanLeftOutput;
            _afCleanRightInput = afCleanRightInput;
            _afCleanRightOutput = afCleanRightOutput;
            _unloadTransferLeftInput = unloadTransferLeftInput;
            _unloadTransferLeftOutput = unloadTransferLeftOutput;
            _unloadTransferRightInput = unloadTransferRightInput;
            _unloadTransferRightOutput = unloadTransferRightOutput;
            _unloadAlignInput = unloadAlignInput;
            _unloadAlignOutput = unloadAlignOutput;
        }

        public void Initialize()
        {
            _inWorkConveyorInput.Initialize();
            _inWorkConveyorOutput.Initialize();

            _outWorkConveyorInput.Initialize();
            _outWorkConveyorOutput.Initialize();

            _robotLoadInput.Initialize();
            _robotLoadOutput.Initialize();

            _vinylCleanInput.Initialize();
            _vinylCleanOutput.Initialize();

            _fixtureAlignInput.Initialize();
            _fixtureAlignOutput.Initialize();

            _removeFilmInput.Initialize();
            _removeFilmOutput.Initialize();

            _transferFixtureInput.Initialize();
            _transferFixtureOutput.Initialize();

            _detachInput.Initialize();
            _detachOutput.Initialize();

            _glassTransferInput.Initialize();
            _glassTransferOutput.Initialize();

            _glassAlignLeftInput.Initialize();
            _glassAlignLeftOutput.Initialize();
            _glassAlignRightInput.Initialize();
            _glassAlignRightOutput.Initialize();

            _transferInShuttleLeftInput.Initialize();
            _transferInShuttleRightInput.Initialize();
            _transferInShuttleLeftOutput.Initialize();
            _transferInShuttleRightOutput.Initialize();

            _wetCleanLeftInput.Initialize();
            _wetCleanLeftOutput.Initialize();
            _wetCleanRightInput.Initialize();
            _wetCleanRightOutput.Initialize();

            _transferRotationLeftInput.Initialize();
            _transferRotationLeftOutput.Initialize();
            _transferRotationRightInput.Initialize();
            _transferRotationRightOutput.Initialize();

            _afCleanLeftInput.Initialize();
            _afCleanLeftOutput.Initialize();
            _afCleanRightInput.Initialize();
            _afCleanRightOutput.Initialize();

            _unloadTransferLeftInput.Initialize();
            _unloadTransferLeftOutput.Initialize();
            _unloadTransferRightInput.Initialize();
            _unloadTransferRightOutput.Initialize();

            _unloadAlignInput.Initialize();
            _unloadAlignOutput.Initialize();
        }

        public void Mappings()
        {
            //InWorkConveyor Input Mapping
            ((VirtualInputDevice<EInWorkConveyorProcessInput>)_inWorkConveyorInput).Mapping((int)EInWorkConveyorProcessInput.ROBOT_PICK_IN_CST_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE);

            //OutWorkConveyor Input Mapping
            ((VirtualInputDevice<EOutWorkConveyorProcessInput>)_outWorkConveyorInput).Mapping((int)EOutWorkConveyorProcessInput.ROBOT_PLACE_OUT_CST_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE);

            //Robot Load Input Mapping
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.IN_CST_READY,
                _inWorkConveyorOutput, (int)EInWorkConveyorProcessOutput.IN_CST_READY);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.OUT_CST_READY,
                _outWorkConveyorOutput, (int)EOutWorkConveyorProcessOutput.OUT_CST_READY);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_LOAD,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_LOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_LOAD_DONE,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_LOAD_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE,
                _vinylCleanOutput, (int)EVinylCleanProcessOutput.VINYL_CLEAN_RECEIVE_UNLOAD_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_REQ_LOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.REMOVE_FILM_REQ_UNLOAD,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.IN_CST_PICK_DONE,
                _inWorkConveyorOutput, (int)EInWorkConveyorProcessOutput.IN_CST_PICK_DONE);
            ((VirtualInputDevice<ERobotLoadProcessInput>)_robotLoadInput).Mapping((int)ERobotLoadProcessInput.OUT_CST_PLACE_DONE,
                _outWorkConveyorOutput, (int)EOutWorkConveyorProcessOutput.OUT_CST_PLACE_DONE);

            //Vinyl Clean Input Mapping
            ((VirtualInputDevice<EVinylCleanProcessInput>)_vinylCleanInput).Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<EVinylCleanProcessInput>)_vinylCleanInput).Mapping((int)EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE);

            //Fixture Align Input Mapping
            ((VirtualInputDevice<EFixtureAlignProcessInput>)_fixtureAlignInput).Mapping((int)EFixtureAlignProcessInput.FIXTURE_ALIGN_LOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.FIXTURE_ALIGN_LOAD_DONE);
            ((VirtualInputDevice<EFixtureAlignProcessInput>)_fixtureAlignInput).Mapping((int)EFixtureAlignProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Remove Film Input Mapping
            ((VirtualInputDevice<ERemoveFilmProcessInput>)_removeFilmInput).Mapping((int)ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE,
                _robotLoadOutput, (int)ERobotLoadProcessOutput.REMOVE_FILM_UNLOAD_DONE);
            ((VirtualInputDevice<ERemoveFilmProcessInput>)_removeFilmInput).Mapping((int)ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);

            //Transfer Fixture Input Mapping
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_ORIGIN_DONE,
                _detachOutput, (int)EDetachProcessOutput.DETACH_ORIGIN_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_DONE,
                _detachOutput, (int)EDetachProcessOutput.DETACH_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.FIXTURE_ALIGN_DONE,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.FIXTURE_ALIGN_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_DONE,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.REMOVE_FILM_DONE);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.ALIGN_TRANSFER_FIXTURE_DONE_RECEIVED,
                _fixtureAlignOutput, (int)EFixtureAlignProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.DETACH_TRANSFER_FIXTURE_DONE_RECEIVED,
                _detachOutput, (int)EDetachProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferFixtureProcessInput>)_transferFixtureInput).Mapping((int)ETransferFixtureProcessInput.REMOVE_FILM_TRANSFER_FIXTURE_DONE_RECEIVED,
                _removeFilmOutput, (int)ERemoveFilmProcessOutput.TRANSFER_FIXTURE_DONE_RECEIVED);

            //Detach Input Mapping
            ((VirtualInputDevice<EDetachProcessInput>)_detachInput).Mapping((int)EDetachProcessInput.FIXTURE_TRANSFER_DONE,
                _transferFixtureOutput, (int)ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE);
            ((VirtualInputDevice<EDetachProcessInput>)_detachInput).Mapping((int)EDetachProcessInput.GLASS_TRANSFER_PICK_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE);

            //Glass Transfer Input Mapping
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS,
                _detachOutput, (int)EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_TRANSFER_PICK_DONE_RECEIVED,
                _detachOutput, (int)EDetachProcessOutput.GLASS_TRANSFER_PICK_DONE_RECEIVED);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_REQ_GLASS,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_REQ_GLASS,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_GLASS);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_LEFT_PLACE_DONE_RECEIVED,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);
            ((VirtualInputDevice<EGlassTransferProcessInput>)_glassTransferInput).Mapping((int)EGlassTransferProcessInput.GLASS_ALIGN_RIGHT_PLACE_DONE_RECEIVED,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PLACE_DONE_RECEIVED);

            //Glass Align Input Mapping
            //Left
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignLeftInput).Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE);
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignLeftInput).Mapping((int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE,
                _transferInShuttleLeftOutput, (int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE);
            //Right
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignRightInput).Mapping((int)EGlassAlignProcessInput.GLASS_TRANSFER_PLACE_DONE,
                _glassTransferOutput, (int)EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE);
            ((VirtualInputDevice<EGlassAlignProcessInput>)_glassAlignRightInput).Mapping((int)EGlassAlignProcessInput.TRANSFER_IN_SHUTTLE_PICK_DONE,
                _transferInShuttleRightOutput, (int)ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_PICK_DONE);

            //TransferInShuttle Input Mapping
            //Left
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                _glassAlignLeftOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD,
                _wetCleanLeftOutput, (int)ECleanProcessOutput.REQ_LOAD);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleLeftInput).Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED,
                _wetCleanLeftOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);
            //Right
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_REQ_PICK,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_REQ_PICK);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.GLASS_ALIGN_PICK_DONE_RECEIVED,
                _glassAlignRightOutput, (int)EGlassAlignProcessOutput.GLASS_ALIGN_PICK_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD,
                _wetCleanRightOutput, (int)ECleanProcessOutput.REQ_LOAD);
            ((VirtualInputDevice<ETransferInShuttleProcessInput>)_transferInShuttleRightInput).Mapping((int)ETransferInShuttleProcessInput.WET_CLEAN_LOAD_DONE_RECEIVED,
                _wetCleanRightOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);


            //Clean Input Mapping
            //WET Clean
            //Left
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanLeftInput).Mapping((int)ECleanProcessInput.LOAD_DONE,
                _transferInShuttleLeftOutput, (int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanLeftInput).Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                _transferRotationLeftOutput, (int)ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanLeftInput).Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                _transferRotationLeftOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK);
            //Right 
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanRightInput).Mapping((int)ECleanProcessInput.LOAD_DONE,
                _transferInShuttleRightOutput, (int)ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanRightInput).Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                _transferRotationRightOutput, (int)ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_wetCleanRightInput).Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                _transferRotationRightOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK);

            //AF Clean
            //Left
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanLeftInput).Mapping((int)ECleanProcessInput.LOAD_DONE,
                _transferRotationLeftOutput, (int)ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanLeftInput).Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                _unloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanLeftInput).Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                _transferRotationLeftOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE);

            //Right
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanRightInput).Mapping((int)ECleanProcessInput.LOAD_DONE,
                _transferRotationRightOutput, (int)ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanRightInput).Mapping((int)ECleanProcessInput.UNLOAD_DONE,
                _unloadTransferRightOutput, (int)EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE);
            ((VirtualInputDevice<ECleanProcessInput>)_afCleanRightInput).Mapping((int)ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE,
                _transferRotationRightOutput, (int)ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE);

            //Transfer Rotation Input Mapping
            //Left
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationLeftInput).Mapping((int)ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD,
                _wetCleanLeftOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationLeftInput).Mapping((int)ETransferRotationProcessInput.WET_CLEAN_UNLOAD_DONE_RECEIVED,
                _wetCleanLeftOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationLeftInput).Mapping((int)ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD,
                _afCleanLeftOutput, (int)ECleanProcessOutput.REQ_LOAD);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationLeftInput).Mapping((int)ETransferRotationProcessInput.AF_CLEAN_LOAD_DONE_RECEIVED,
                _afCleanLeftOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //Right
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationRightInput).Mapping((int)ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD,
            _wetCleanRightOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationRightInput).Mapping((int)ETransferRotationProcessInput.WET_CLEAN_UNLOAD_DONE_RECEIVED,
                _wetCleanRightOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationRightInput).Mapping((int)ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD,
                _afCleanRightOutput, (int)ECleanProcessOutput.REQ_LOAD);
            ((VirtualInputDevice<ETransferRotationProcessInput>)_transferRotationRightInput).Mapping((int)ETransferRotationProcessInput.AF_CLEAN_LOAD_DONE_RECEIVED,
                _afCleanRightOutput, (int)ECleanProcessOutput.LOAD_DONE_RECEIVED);

            //Unload Transfer Input Mapping
            //Left
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferLeftInput).Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD,
                        _afCleanLeftOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferLeftInput).Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_UNLOAD_DONE_RECEIVED,
                        _afCleanLeftOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferLeftInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING,
                        _unloadTransferRightOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferLeftInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_READY,
                        _unloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferLeftInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED,
                        _unloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED);

            //Right
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferRightInput).Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD,
                    _afCleanRightOutput, (int)ECleanProcessOutput.REQ_UNLOAD);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferRightInput).Mapping((int)EUnloadTransferProcessInput.AF_CLEAN_UNLOAD_DONE_RECEIVED,
                        _afCleanRightOutput, (int)ECleanProcessOutput.UNLOAD_DONE_RECEIVED);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferRightInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING,
                        _unloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferRightInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_READY,
                        _unloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY);
            ((VirtualInputDevice<EUnloadTransferProcessInput>)_unloadTransferRightInput).Mapping((int)EUnloadTransferProcessInput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED,
                        _unloadAlignOutput, (int)EUnloadAlignProcessOutput.UNLOAD_ALIGN_PLACE_DONE_RECEIVED);

            //Unload Align Input Mapping
            ((VirtualInputDevice<EUnloadAlignProcessInput>)_unloadAlignInput).Mapping((int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_LEFT_PLACE_DONE,
                    _unloadTransferLeftOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE);
            ((VirtualInputDevice<EUnloadAlignProcessInput>)_unloadAlignInput).Mapping((int)EUnloadAlignProcessInput.UNLOAD_TRANSFER_RIGHT_PLACE_DONE,
                    _unloadTransferRightOutput, (int)EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE);
        }
    }
}
