using EQX.Core.InOut;
using EQX.InOut;
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
        private readonly IDInputDevice _inConveyorInput;
        private readonly IDInputDevice _inWorkConveyorInput;
        private readonly IDOutputDevice _inWorkConveyorOutput;
        private readonly IDInputDevice _bufferConveyorInput;
        private readonly IDOutputDevice _bufferConveyorOutput;
        private readonly IDInputDevice _outWorkConveyorInput;
        private readonly IDOutputDevice _outWorkConveyorOutput;
        private readonly IDInputDevice _outConveyorInput;
        private readonly IDOutputDevice _outConveyorOutput;
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
        private readonly IDInputDevice _robotUnloadInput;
        private readonly IDOutputDevice _robotUnloadOutput;

        public VirtualIO([FromKeyedServices("InConveyorInput")] IDInputDevice inConveyorInput,
                         [FromKeyedServices("InWorkConveyorInput")] IDInputDevice inWorkConveyorInput,
                         [FromKeyedServices("InWorkConveyorOutput")] IDOutputDevice inWorkConveyorOutput,
                         [FromKeyedServices("BufferConveyorInput")] IDInputDevice bufferConveyorInput,
                         [FromKeyedServices("BufferConveyorOutput")] IDOutputDevice bufferConveyorOutput,
                         [FromKeyedServices("OutWorkConveyorInput")] IDInputDevice outWorkConveyorInput,
                         [FromKeyedServices("OutWorkConveyorOutput")] IDOutputDevice outWorkConveyorOutput,
                         [FromKeyedServices("OutConveyorInput")] IDInputDevice outConveyorInput,
                         [FromKeyedServices("OutConveyorOutput")] IDOutputDevice outConveyorOutput,
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
                         [FromKeyedServices("UnloadAlignOutput")] IDOutputDevice unloadAlignOutput,
                         [FromKeyedServices("RobotUnloadInput")] IDInputDevice robotUnloadInput,
                         [FromKeyedServices("RobotUnloadOutput")] IDOutputDevice robotUnloadOutput)
        {
            _inConveyorInput = inConveyorInput;
            _inWorkConveyorInput = inWorkConveyorInput;
            _inWorkConveyorOutput = inWorkConveyorOutput;
            _bufferConveyorInput = bufferConveyorInput;
            _bufferConveyorOutput = bufferConveyorOutput;
            _outWorkConveyorInput = outWorkConveyorInput;
            _outWorkConveyorOutput = outWorkConveyorOutput;
            _outConveyorInput = outConveyorInput;
            _outConveyorOutput = outConveyorOutput;
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

            _robotUnloadInput = robotUnloadInput;
            _robotUnloadOutput = robotUnloadOutput;
        }

        public void Initialize()
        {
            _inConveyorInput.Initialize();

            _inWorkConveyorInput.Initialize();
            _inWorkConveyorOutput.Initialize();

            _bufferConveyorInput.Initialize();
            _bufferConveyorOutput.Initialize();

            _outWorkConveyorInput.Initialize();
            _outWorkConveyorOutput.Initialize();

            _outConveyorInput.Initialize();
            _outConveyorOutput.Initialize();

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

            _robotUnloadInput.Initialize();
            _robotUnloadOutput.Initialize();
        }

        public void Mappings()
        {
            //InConveyor Input Mapping
            _inConveyorInput[EInConveyorProcessInput.REQUEST_CST_IN].
                MapTo(_inWorkConveyorOutput[EWorkConveyorProcessOutput.REQUEST_CST_IN]);

            //InWorkConveyor Input Mapping
            _inWorkConveyorInput[EWorkConveyorProcessInput.ROBOT_PICK_PLACE_CST_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.ROBOT_PICK_IN_CST_DONE]);
            _inWorkConveyorInput[EWorkConveyorProcessInput.DOWN_STREAM_READY].
                MapTo(_bufferConveyorOutput[EBufferConveyorProcessOutput.BUFFER_CONVEYOR_READY]);
            _inWorkConveyorInput[EWorkConveyorProcessInput.ROBOT_ORIGIN_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.ROBOT_ORIGIN_DONE]);

            //BufferConveyor Input Mapping
            _bufferConveyorInput[EBufferConveyorProcessInput.IN_WORK_CONVEYOR_REQUEST_CST_OUT].
                MapTo(_inWorkConveyorOutput[EWorkConveyorProcessOutput.REQUEST_CST_OUT]);
            _bufferConveyorInput[EBufferConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_IN].
                MapTo(_outWorkConveyorOutput[EWorkConveyorProcessOutput.REQUEST_CST_IN]);

            //OutWorkConveyor Input Mapping
            _outWorkConveyorInput[EWorkConveyorProcessInput.ROBOT_PICK_PLACE_CST_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.ROBOT_PLACE_OUT_CST_DONE]);
            _outWorkConveyorInput[EWorkConveyorProcessInput.DOWN_STREAM_READY].
                MapTo(_outConveyorOutput[EOutConveyorProcessOutput.OUT_CONVEYOR_READY]);
            _outWorkConveyorInput[EWorkConveyorProcessInput.ROBOT_ORIGIN_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.ROBOT_ORIGIN_DONE]);

            //OutConveyor Input Mapping
            _outConveyorInput[EOutConveyorProcessInput.OUT_WORK_CONVEYOR_REQUEST_CST_OUT].
                MapTo(_outWorkConveyorOutput[EWorkConveyorProcessOutput.REQUEST_CST_OUT]);

            //Robot Load Input Mapping
            _robotLoadInput[ERobotLoadProcessInput.IN_CST_READY].
                MapTo(_inWorkConveyorOutput[EWorkConveyorProcessOutput.CST_READY]);
            _robotLoadInput[ERobotLoadProcessInput.OUT_CST_READY].
                MapTo(_outWorkConveyorOutput[EWorkConveyorProcessOutput.CST_READY]);
            _robotLoadInput[ERobotLoadProcessInput.VINYL_CLEAN_REQ_LOAD].
                MapTo(_vinylCleanOutput[EVinylCleanProcessOutput.VINYL_CLEAN_REQ_LOAD]);
            _robotLoadInput[ERobotLoadProcessInput.VINYL_CLEAN_REQ_UNLOAD].
                MapTo(_vinylCleanOutput[EVinylCleanProcessOutput.VINYL_CLEAN_REQ_UNLOAD]);
            _robotLoadInput[ERobotLoadProcessInput.FIXTURE_ALIGN_REQ_LOAD].
                MapTo(_fixtureAlignOutput[EFixtureAlignProcessOutput.FIXTURE_ALIGN_REQ_LOAD]);
            _robotLoadInput[ERobotLoadProcessInput.REMOVE_FILM_REQ_UNLOAD].
                MapTo(_removeFilmOutput[ERemoveFilmProcessOutput.REMOVE_FILM_REQ_UNLOAD]);

            //Vinyl Clean Input Mapping
            _vinylCleanInput[EVinylCleanProcessInput.VINYL_CLEAN_LOAD_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.VINYL_CLEAN_LOAD_DONE]);
            _vinylCleanInput[EVinylCleanProcessInput.VINYL_CLEAN_UNLOAD_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.VINYL_CLEAN_UNLOAD_DONE]);

            //Fixture Align Input Mapping
            _fixtureAlignInput[EFixtureAlignProcessInput.FIXTURE_ALIGN_LOAD_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.FIXTURE_ALIGN_LOAD_DONE]);
            _fixtureAlignInput[EFixtureAlignProcessInput.FIXTURE_TRANSFER_DONE].
                MapTo(_transferFixtureOutput[ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE]);

            //Remove Film Input Mapping
            _removeFilmInput[ERemoveFilmProcessInput.REMOVE_FILM_UNLOAD_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.REMOVE_FILM_UNLOAD_DONE]);
            _removeFilmInput[ERemoveFilmProcessInput.FIXTURE_TRANSFER_DONE].
                MapTo(_transferFixtureOutput[ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE]);
            _removeFilmInput[ERemoveFilmProcessInput.ROBOT_ORIGIN_DONE].
                MapTo(_robotLoadOutput[ERobotLoadProcessOutput.ROBOT_ORIGIN_DONE]);

            //Transfer Fixture Input Mapping
            _transferFixtureInput[ETransferFixtureProcessInput.DETACH_ORIGIN_DONE].
                MapTo(_detachOutput[EDetachProcessOutput.DETACH_ORIGIN_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.DETACH_READY_DONE].
                MapTo(_detachOutput[EDetachProcessOutput.DETACH_READY_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.DETACH_DONE].
                MapTo(_detachOutput[EDetachProcessOutput.DETACH_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.FIXTURE_ALIGN_DONE].
                MapTo(_fixtureAlignOutput[EFixtureAlignProcessOutput.FIXTURE_ALIGN_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.REMOVE_FILM_DONE].
                MapTo(_removeFilmOutput[ERemoveFilmProcessOutput.REMOVE_FILM_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.REMOVE_FILM_ORIGIN_DONE].
                MapTo(_removeFilmOutput[ERemoveFilmProcessOutput.REMOVE_FILM_ORIGIN_DONE]);
            _transferFixtureInput[ETransferFixtureProcessInput.REMOVE_FILM_READY_DONE].
                MapTo(_removeFilmOutput[ERemoveFilmProcessOutput.REMOVE_FILM_READY_DONE]);

            //Detach Input Mapping
            _detachInput[EDetachProcessInput.FIXTURE_TRANSFER_DONE].
                MapTo(_transferFixtureOutput[ETransferFixtureProcessOutput.FIXTURE_TRANSFER_DONE]);
            _detachInput[EDetachProcessInput.GLASS_TRANSFER_PICK_DONE].
                MapTo(_glassTransferOutput[EGlassTransferProcessOutput.GLASS_TRANSFER_PICK_DONE]);

            //Glass Transfer Input Mapping
            _glassTransferInput[EGlassTransferProcessInput.DETACH_REQ_UNLOAD_GLASS].
                MapTo(_detachOutput[EDetachProcessOutput.DETACH_REQ_UNLOAD_GLASS]);
            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_L_ORIGIN_DONE].
                MapTo(_transferInShuttleLeftOutput[ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_ORIGIN_DONE]);
            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_R_ORIGIN_DONE].
                MapTo(_transferInShuttleRightOutput[ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_ORIGIN_DONE]);

            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_LEFT_GLASS_REQUEST].
                MapTo(_transferInShuttleLeftOutput[ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_GLASS_REQUEST]);
            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_RIGHT_GLASS_REQUEST].
                MapTo(_transferInShuttleRightOutput[ETransferInShuttleProcessOutput.TRANSFER_IN_SHUTTLE_GLASS_REQUEST]);

            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_L_IN_SAFE_POS].
                MapTo(_transferInShuttleLeftOutput[ETransferInShuttleProcessOutput.IN_SAFETY_POSITION]);
            _glassTransferInput[EGlassTransferProcessInput.TRANSFER_IN_SHUTTLE_R_IN_SAFE_POS].
                MapTo(_transferInShuttleRightOutput[ETransferInShuttleProcessOutput.IN_SAFETY_POSITION]);

            // TransferInShuttle Input Mapping
            // Left
            _transferInShuttleLeftInput[ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD].
                MapTo(_wetCleanLeftOutput[ECleanProcessOutput.REQ_LOAD]);
            _transferInShuttleLeftInput[ETransferInShuttleProcessInput.GLASS_TRANSFER_PLACE_DONE].
                MapTo(_glassTransferOutput[EGlassTransferProcessOutput.GLASS_TRANSFER_LEFT_PLACE_DONE]);

            // Right
            _transferInShuttleRightInput[ETransferInShuttleProcessInput.WET_CLEAN_REQ_LOAD].
                MapTo(_wetCleanRightOutput[ECleanProcessOutput.REQ_LOAD]);
            _transferInShuttleRightInput[ETransferInShuttleProcessInput.GLASS_TRANSFER_PLACE_DONE].
                MapTo(_glassTransferOutput[EGlassTransferProcessOutput.GLASS_TRANSFER_RIGHT_PLACE_DONE]);

            // Clean Input Mapping
            // WET Clean
            // Left
            _wetCleanLeftInput[ECleanProcessInput.LOAD_DONE].
                MapTo(_transferInShuttleLeftOutput[ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE]);
            _wetCleanLeftInput[ECleanProcessInput.UNLOAD_DONE].
                MapTo(_transferRotationLeftOutput[ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE]);
            _wetCleanLeftInput[ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE].
                MapTo(_transferRotationLeftOutput[ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK]);
            _wetCleanLeftInput[ECleanProcessInput.TRANSFER_IN_SHUTTLE_IN_SAFE_POS].
                MapTo(_transferInShuttleLeftOutput[ETransferInShuttleProcessOutput.IN_SAFETY_POSITION]);

            // Right
            _wetCleanRightInput[ECleanProcessInput.LOAD_DONE].
                MapTo(_transferInShuttleRightOutput[ETransferInShuttleProcessOutput.WET_CLEAN_LOAD_DONE]);
            _wetCleanRightInput[ECleanProcessInput.UNLOAD_DONE].
                MapTo(_transferRotationRightOutput[ETransferRotationProcessOutput.WET_CLEAN_UNLOAD_DONE]);
            _wetCleanRightInput[ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE].
                MapTo(_transferRotationRightOutput[ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PICK]);
            _wetCleanRightInput[ECleanProcessInput.TRANSFER_IN_SHUTTLE_IN_SAFE_POS].
                MapTo(_transferInShuttleRightOutput[ETransferInShuttleProcessOutput.IN_SAFETY_POSITION]);

            //AF Clean
            //Left
            _afCleanLeftInput[ECleanProcessInput.LOAD_DONE].
                MapTo(_transferRotationLeftOutput[ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE]);
            _afCleanLeftInput[ECleanProcessInput.UNLOAD_DONE].
                MapTo(_unloadTransferLeftOutput[EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE]);
            _afCleanLeftInput[ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE].
                MapTo(_transferRotationLeftOutput[ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE]);

            //Right
            _afCleanRightInput[ECleanProcessInput.LOAD_DONE].
                MapTo(_transferRotationRightOutput[ETransferRotationProcessOutput.AF_CLEAN_LOAD_DONE]);
            _afCleanRightInput[ECleanProcessInput.UNLOAD_DONE].
                MapTo(_unloadTransferRightOutput[EUnloadTransferProcessOutput.AF_CLEAN_UNLOAD_DONE]);
            _afCleanRightInput[ECleanProcessInput.TRANSFER_ROTATION_READY_PICK_PLACE].
                MapTo(_transferRotationRightOutput[ETransferRotationProcessOutput.TRANSFER_ROTATION_READY_PLACE]);

            //Transfer Rotation Input Mapping
            //Left
            _transferRotationLeftInput[ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD].
                MapTo(_wetCleanLeftOutput[ECleanProcessOutput.REQ_UNLOAD]);
            _transferRotationLeftInput[ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD].
                MapTo(_afCleanLeftOutput[ECleanProcessOutput.REQ_LOAD]);

            //Right
            _transferRotationRightInput[ETransferRotationProcessInput.WET_CLEAN_REQ_UNLOAD].
                MapTo(_wetCleanRightOutput[ECleanProcessOutput.REQ_UNLOAD]);
            _transferRotationRightInput[ETransferRotationProcessInput.AF_CLEAN_REQ_LOAD].
                MapTo(_afCleanRightOutput[ECleanProcessOutput.REQ_LOAD]);

            //Unload Transfer Input Mapping
            //Left
            _unloadTransferLeftInput[EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD].
                MapTo(_afCleanLeftOutput[ECleanProcessOutput.REQ_UNLOAD]);
            _unloadTransferLeftInput[EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING].
                MapTo(_unloadTransferRightOutput[EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING]);
            _unloadTransferLeftInput[EUnloadTransferProcessInput.UNLOAD_ALIGN_READY].
                MapTo(_unloadAlignOutput[EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY]);

            //Right
            _unloadTransferRightInput[EUnloadTransferProcessInput.AF_CLEAN_REQ_UNLOAD].
                MapTo(_afCleanRightOutput[ECleanProcessOutput.REQ_UNLOAD]);
            _unloadTransferRightInput[EUnloadTransferProcessInput.UNLOAD_TRANSFER_UNLOADING].
                MapTo(_unloadTransferLeftOutput[EUnloadTransferProcessOutput.UNLOAD_TRANSFER_UNLOADING]);
            _unloadTransferRightInput[EUnloadTransferProcessInput.UNLOAD_ALIGN_READY].
                MapTo(_unloadAlignOutput[EUnloadAlignProcessOutput.UNLOAD_ALIGN_READY]);

            // Unload Align Input Mapping
            _unloadAlignInput[EUnloadAlignProcessInput.UNLOAD_TRANSFER_LEFT_PLACE_DONE].
                MapTo(_unloadTransferLeftOutput[EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE]);
            _unloadAlignInput[EUnloadAlignProcessInput.UNLOAD_TRANSFER_RIGHT_PLACE_DONE].
                MapTo(_unloadTransferRightOutput[EUnloadTransferProcessOutput.UNLOAD_TRANSFER_PLACE_DONE]);
            _unloadAlignInput[EUnloadAlignProcessInput.ROBOT_UNLOAD_PICK_DONE].
                MapTo(_robotUnloadOutput[ERobotUnloadProcessOutput.ROBOT_UNLOAD_PICK_DONE]);

            // Robot Unload Input Mapping
            _robotUnloadInput[ERobotUnloadProcessInput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD].
                MapTo(_unloadAlignOutput[EUnloadAlignProcessOutput.UNLOAD_ALIGN_REQ_ROBOT_UNLOAD]);
        }
    }
}
